using System;
using System.Linq.Expressions;

using Toe.CircularArrayQueue;

namespace Toe.Messaging.Types
{
	public class StringBinarySerializer : ITypeBinarySerializer
	{
		#region Constants and Fields

		public static readonly ITypeBinarySerializer Instance = new StringBinarySerializer();

		#endregion

		#region Public Properties

		public int FixedSize
		{
			get
			{
				return 1;
			}
		}

		public int PropertyType
		{
			get
			{
				return Messaging.PropertyType.String;
			}
		}

		#endregion

		#region Public Methods and Operators

		public Expression BuildDeserializeExpression(MessageMemberInfo member, BinarySerilizationContext context)
		{
			return
				Expression.Call(
					((Func<IMessageQueue, int, string>)CircularArrayQueue.ExtensionMethods.ReadStringContent).Method,
					context.QueueParameter,
					Expression.Add(
						context.PositionParameter,
						Expression.Call(
							context.QueueParameter,
							MessageQueueMethods.ReadInt32,
							Expression.Add(Expression.Constant(member.Offset), context.PositionParameter))));
		}

		public Expression BuildDynamicSizeEvaluator(MessageMemberInfo member, BinarySerilizationContext context)
		{
			return
				Expression.RightShift(
					Expression.Add(
						Expression.Constant(4),
						Expression.Call(
							((Func<string, int>)CircularArrayQueue.ExtensionMethods.GetByteCount).Method,
							member.GetProperty(context.MessageParameter))),
					Expression.Constant(2));
		}

		public void BuildSerializeExpression(MessageMemberInfo member, BinarySerilizationContext context)
		{
			context.Code.Add(
				Expression.Call(
					context.QueueParameter,
					MessageQueueMethods.WriteInt32,
					Expression.Add(context.PositionParameter, Expression.Constant(member.Offset)),
					Expression.Subtract(context.DynamicPositionParameter, context.PositionParameter)));
			context.Code.Add(
				Expression.Assign(
					context.DynamicPositionParameter,
					Expression.Call(
						((Func<IMessageQueue, int, string, int>)CircularArrayQueue.ExtensionMethods.WriteStringContent).Method,
						context.QueueParameter,
						context.DynamicPositionParameter,
						member.GetProperty(context.MessageParameter))));
		}

		public bool CanHandleType(Type fieldType)
		{
			return typeof(string) == fieldType;
		}

		#endregion
	}
}