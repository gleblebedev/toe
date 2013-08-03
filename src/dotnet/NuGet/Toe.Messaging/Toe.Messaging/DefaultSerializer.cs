using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Toe.CircularArrayQueue;

namespace Toe.Messaging
{
	public class DefaultSerializer<T> : IMessageSerializer<T>
	{
		#region Constants and Fields

		private readonly Action<IMessageQueue, int, T> deserialize;

		private readonly int messageId;

		private readonly Action<IMessageQueue, T> serialize;

		#endregion

		#region Constructors and Destructors

		public DefaultSerializer(MessageRegistry messageRegistry, int messageId)
		{
			this.messageId = messageId;
			var description = messageRegistry.DefineMessage(this.messageId);

			var allMembers = this.GetMembers(typeof(T));

			var fixedSize = 0;

			var queueParameter = Expression.Parameter(typeof(IMessageQueue), "q");
			var positionParameter = Expression.Parameter(typeof(int), "p");
			var dynamicPositionParameter = Expression.Parameter(typeof(int), "d");
			var messageParameter = Expression.Parameter(typeof(T), "m");
			{
				Expression dynamicSizeEval = null;
				foreach (var member in allMembers)
				{
					member.Offset = fixedSize;
					fixedSize += member.Serializer.FixedSize;
					Expression dynamicSize = member.Serializer.BuildDynamicSizeEvaluator(member, messageParameter);
					if (dynamicSize != null)
					{
						dynamicSizeEval = dynamicSizeEval == null ? dynamicSize : Expression.Add(dynamicSizeEval, dynamicSize);
					}
				}

				var items = new List<Expression>(allMembers.Count + 3)
					{
						Expression.Assign(
							positionParameter,
							Expression.Call(
								queueParameter,
								MessageQueueMethods.Allocate,
								new Expression[] { Expression.Add(Expression.Constant(fixedSize), dynamicSizeEval ?? Expression.Constant(0)) })),
						Expression.Assign(dynamicPositionParameter, Expression.Add(positionParameter, Expression.Constant(fixedSize)))
					};
				items.AddRange(
					allMembers.Select(
						member =>
						member.Serializer.BuildSerializeExpression(
							member, positionParameter, dynamicPositionParameter, queueParameter, messageParameter)));
				items.Add(Expression.Call(queueParameter, MessageQueueMethods.Commit, new Expression[] { positionParameter }));
				var body = Expression.Block(new[] { positionParameter, dynamicPositionParameter }, items);
				var serializeExpression = Expression.Lambda<Action<IMessageQueue, T>>(
					body, new[] { queueParameter, messageParameter });

				this.serialize = serializeExpression.Compile();
			}
			{
				var items = new List<Expression>(allMembers.Count + 3);
				items.AddRange(
					allMembers.Select(
						member =>
						member.Serializer.BuildDeserializeExpression(member, positionParameter, queueParameter, messageParameter)));
				var body = Expression.Block(new ParameterExpression[] { }, items);
				var deserializeExpression = Expression.Lambda<Action<IMessageQueue, int, T>>(
					body, new[] { queueParameter, positionParameter, messageParameter });
				this.deserialize = deserializeExpression.Compile();
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
					x => new MessageMemberInfo(x)).Where(a => a.Type != PropertyType.Unknown).ToList();
			all.AddRange(
				t.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Select(
					x => new MessageMemberInfo(x)).Where(a => a.Type != PropertyType.Unknown).ToList());
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