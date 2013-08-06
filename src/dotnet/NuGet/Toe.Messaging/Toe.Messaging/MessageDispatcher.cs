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
	public class MessageDispatcher<T>
	{
		#region Constants and Fields

		private readonly MessageRegistry messageRegistry;

		private readonly Tuple<int, Func<IMessageQueue, int, T, bool>>[] methodRegistry;

		private readonly TypeRegistry typeRegistry;

		#endregion

		#region Constructors and Destructors

		public MessageDispatcher(MessageRegistry messageRegistry, TypeRegistry typeRegistry)
		{
			this.messageRegistry = messageRegistry;
			this.typeRegistry = typeRegistry;

			var listOfMethods = new List<Tuple<int, Func<IMessageQueue, int, T, bool>>>();
			foreach (var methodInfo in typeof(T).GetMethods(BindingFlags.Public | BindingFlags.Instance))
			{
				if (methodInfo.DeclaringType == typeof(object))
				{
					continue;
				}

				var id = MessageIdAttribute.Get(methodInfo);
				var deserializer = this.BuildDeserializer(methodInfo, id);
				if (deserializer != null)
				{
					listOfMethods.Add(new Tuple<int, Func<IMessageQueue, int, T, bool>>(id, deserializer));
				}
			}
			this.methodRegistry = listOfMethods.OrderBy(x => x.Item1).ToArray();
		}

		#endregion

		#region Public Methods and Operators

		public bool Dispatch(IMessageQueue messageQueue, int position, int messageId, T api)
		{
			var m = this.BinarySearch(messageId, 0, this.methodRegistry.Length - 1);
			if (m == null)
			{
				return false;
			}
			return m(messageQueue, position, api);
		}

		#endregion

		#region Methods

		private Func<IMessageQueue, int, T, bool> BinarySearch(int messageId, int leftIndex, int rightIndex)
		{
			retry:
			if (rightIndex < leftIndex)
			{
				return null;
			}
			// calculate midpoint to cut set in half
			int midIndex = (leftIndex + rightIndex) >> 1;

			var key = this.methodRegistry[midIndex].Item1;

			// three-way comparison
			if (key > messageId)
			{
				// key is in lower subset
				rightIndex = midIndex - 1;
				goto retry;
			}
			if (key < messageId)
			{
				// key is in upper subset
				leftIndex = midIndex + 1;
				goto retry;
			}
			// key has been found
			return this.methodRegistry[midIndex].Item2;
		}

		private Func<IMessageQueue, int, T, bool> BuildDeserializer(MethodInfo methodInfo, int messageId)
		{
			Func<IMessageQueue, int, T, bool> res = null;
			var methodParameters = methodInfo.GetParameters();
			var messageParameter = methodParameters.FirstOrDefault(x => typeof(Message).IsAssignableFrom(x.ParameterType));
			if (messageParameter != null)
			{
				res = this.BuildDeserializerWithClass(methodInfo, messageParameter, messageId);
				if (res != null)
				{
					return res;
				}
			}
			var all = this.GetTypeMembers(methodParameters.Where(x => x.ParameterType != typeof(IMessageQueue)));

			var messageQueueParameter = Expression.Parameter(typeof(IMessageQueue), "q");
			var positionParameter = Expression.Parameter(typeof(int), "p");
			var apiParameter = Expression.Parameter(typeof(T), "api");
			var callArgs = new List<Expression>();
			var fixedSize = 0;
			var messageDefinition = this.messageRegistry.DefineMessage(messageId);
			foreach (var member in all)
			{
				member.Offset = fixedSize;
				fixedSize += member.Serializer.FixedSize;
				messageDefinition.DefineProperty(member.Name, member.PropertyType, member.Offset, member.Serializer.FixedSize);
			}

			foreach (var methodParameter in methodParameters)
			{
				callArgs.Add(Expression.New(methodParameter.ParameterType));
			}
			var body = Expression.Call(apiParameter, methodInfo, callArgs);
			return
				Expression.Lambda<Func<IMessageQueue, int, T, bool>>(
					body, new[] { messageQueueParameter, positionParameter, apiParameter }).Compile();
		}

		private Func<IMessageQueue, int, T, bool> BuildDeserializerWithClass(
			MethodInfo methodInfo, ParameterInfo messageParameter, int messageId)
		{
			var serializer = Activator.CreateInstance(
				typeof(DefaultSerializer<>).MakeGenericType(messageParameter.ParameterType),
				new object[] { this.messageRegistry, this.typeRegistry, });
			var deserializeMethod = serializer.GetType().GetMethod("Deserialize");

			var messageQueueParameter = Expression.Parameter(typeof(IMessageQueue), "q");
			var positionParameter = Expression.Parameter(typeof(int), "p");
			var apiParameter = Expression.Parameter(typeof(T), "api");
			List<Expression> callArgs = new List<Expression>();
			foreach (var parameterInfo in methodInfo.GetParameters())
			{
				if (parameterInfo == messageParameter)
				{
					callArgs.Add(Expression.New(messageParameter.ParameterType));
					continue;
				}
				if (parameterInfo.ParameterType == typeof(IMessageQueue))
				{
					callArgs.Add(messageQueueParameter);
					continue;
				}
				throw new NotImplementedException();
			}
			var body = Expression.Call(apiParameter, methodInfo, callArgs);
			return
				Expression.Lambda<Func<IMessageQueue, int, T, bool>>(
					body, new[] { messageQueueParameter, positionParameter, apiParameter }).Compile();
		}

		private IEnumerable<MessageMemberInfo> GetTypeMembers(IEnumerable<ParameterInfo> t)
		{
			var all =
				t.Select(x => new MessageMemberInfo(x, this.typeRegistry)).Where(a => a.PropertyType != PropertyType.Unknown).ToList
					();
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