using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Toe.CircularArrayQueue;
using Toe.Messaging.Attributes;
using Toe.Messaging.Types;

namespace Toe.Messaging
{
	public class DefaultSerializer<T> : IMessageSerializer<T>
	{
		#region Constants and Fields

		private readonly int messageId;

		private readonly TypeRegistry typeRegistry;

		private Action<IMessageQueue, int, T> deserialize;

		private Action<IMessageQueue, T> serialize;

		#endregion

		#region Constructors and Destructors

		public DefaultSerializer(MessageRegistry messageRegistry, TypeRegistry typeRegistry)
			: this(messageRegistry, typeRegistry, MessageIdAttribute.Get(typeof(T)))
		{
		}

		public DefaultSerializer(MessageRegistry messageRegistry, TypeRegistry typeRegistry, int messageId)
		{
			this.messageId = messageId;
			this.typeRegistry = typeRegistry;
			var description = messageRegistry.DefineMessage(this.messageId);

			if (typeof(T).BaseType != typeof(object))
			{
				//TODO: register base type.
			}

			var allMembers = this.GetMembers(typeof(T));

			this.BuildSerializer(allMembers);
			this.BuildDeserializer(allMembers);
			this.DefineMessageProperties(allMembers, description);
		}

		#endregion

		#region Public Properties

		public int MessageId
		{
			get
			{
				return this.messageId;
			}
		}

		#endregion

		#region Public Methods and Operators

		public void Deserialize(IMessageQueue queue, int pos, T value)
		{
			this.deserialize(queue, pos, value);
		}

		public void Serialize(IMessageQueue queue, T value)
		{
			this.serialize(queue, value);
		}

		#endregion

		#region Explicit Interface Methods

		void IMessageSerializer.Deserialize(IMessageQueue queue, int pos, object value)
		{
			this.Deserialize(queue, pos, (T)value);
		}

		void IMessageSerializer.Serialize(IMessageQueue queue, object value)
		{
			this.Serialize(queue, (T)value);
		}

		#endregion

		#region Methods

		private void BuildDeserializer(List<MessageMemberInfo> allMembers)
		{
			var queueParameter = Expression.Parameter(typeof(IMessageQueue), "q");
			var positionParameter = Expression.Parameter(typeof(int), "p");
			var dynamicPositionParameter = Expression.Parameter(typeof(int), "d");
			var messageParameter = Expression.Parameter(typeof(T), "m");

			var context = new BinarySerilizationContext(
				queueParameter, messageParameter, positionParameter, dynamicPositionParameter);

			context.Code.Capacity = allMembers.Count + 8;

			foreach (var memberInfo in allMembers)
			{
				memberInfo.Serializer.BuildDeserializeExpression(memberInfo, context);
			}
			var body = Expression.Block(context.LocalVariables, context.Code);
			var deserializeExpression = Expression.Lambda<Action<IMessageQueue, int, T>>(
				body, new[] { queueParameter, positionParameter, messageParameter });
			this.deserialize = deserializeExpression.Compile();
		}

		private int BuildSerializer(List<MessageMemberInfo> allMembers)
		{
			var fixedSize = 0;

			var queueParameter = Expression.Parameter(typeof(IMessageQueue), "q");
			var positionParameter = Expression.Parameter(typeof(int), "p");
			var dynamicPositionParameter = Expression.Parameter(typeof(int), "d");
			var messageParameter = Expression.Parameter(typeof(T), "m");

			var context = new BinarySerilizationContext(
				queueParameter, messageParameter, positionParameter, dynamicPositionParameter);
			context.Code.Capacity = allMembers.Count + 8;
			context.LocalVariables.Add(positionParameter);
			context.LocalVariables.Add(dynamicPositionParameter);

			Expression dynamicSizeEval = null;
			foreach (var member in allMembers)
			{
				if (member.Serializer == null)
				{
					throw new ArgumentException(string.Format("Can't serialize property {0}", member));
				}
				member.Offset = fixedSize;
				fixedSize += member.Serializer.FixedSize;
				Expression dynamicSize = member.Serializer.BuildDynamicSizeEvaluator(member, context);
				if (dynamicSize != null)
				{
					dynamicSizeEval = dynamicSizeEval == null ? dynamicSize : Expression.Add(dynamicSizeEval, dynamicSize);
				}
			}
			context.Code.Add(
				Expression.Assign(
					positionParameter,
					Expression.Call(
						queueParameter,
						MessageQueueMethods.Allocate,
						new Expression[] { Expression.Add(Expression.Constant(fixedSize), dynamicSizeEval ?? Expression.Constant(0)) })));
			context.Code.Add(
				Expression.Assign(dynamicPositionParameter, Expression.Add(positionParameter, Expression.Constant(fixedSize))));

			foreach (var memberInfo in allMembers)
			{
				memberInfo.Serializer.BuildSerializeExpression(memberInfo, context);
			}
			context.Code.Add(Expression.Call(queueParameter, MessageQueueMethods.Commit, new Expression[] { positionParameter }));
			var body = Expression.Block(context.LocalVariables, context.Code);
			var serializeExpression = Expression.Lambda<Action<IMessageQueue, T>>(
				body, new[] { queueParameter, messageParameter });

			this.serialize = serializeExpression.Compile();
			return fixedSize;
		}

		private void DefineMessageProperties(IEnumerable<MessageMemberInfo> allMembers, IMessageDescription description)
		{
			foreach (var member in allMembers)
			{
				description.DefineProperty(member.Name, member.PropertyType, member.Offset, member.Serializer.FixedSize);
			}
		}

		private List<MessageMemberInfo> GetMembers(Type type)
		{
			if (type == typeof(object))
			{
				return new List<MessageMemberInfo>();
			}
			var baseList = this.GetMembers(type.BaseType);
			baseList.AddRange(this.GetTypeMembers(type));
			return baseList;
		}

		private IEnumerable<MessageMemberInfo> GetTypeMembers(Type t)
		{
			var all =
				t.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Select(
					x => new MessageMemberInfo(x, this.typeRegistry)).Where(a => a.PropertyType != PropertyType.Unknown).ToList();
			all.AddRange(
				t.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Select(
					x => new MessageMemberInfo(x, this.typeRegistry)).Where(a => a.PropertyType != PropertyType.Unknown).ToList());
			all.Sort(
				(a, b) =>
					{
						if (a.Order < b.Order)
						{
							return -1;
						}
						if (a.Order > b.Order)
						{
							return 1;
						}
						return String.CompareOrdinal(a.Name, b.Name);
					});
			return all;
		}

		#endregion
	}
}