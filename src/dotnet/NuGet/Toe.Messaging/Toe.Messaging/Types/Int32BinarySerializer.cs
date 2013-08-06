using System;
using System.Linq.Expressions;

namespace Toe.Messaging.Types
{
	public class Int32BinarySerializer : ITypeBinarySerializer
	{
		#region Constants and Fields

		public static readonly ITypeBinarySerializer Instance = new Int32BinarySerializer();

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
				return Messaging.PropertyType.Int32;
			}
		}

		#endregion

		#region Public Methods and Operators

		public void BuildDeserializeExpression(MessageMemberInfo member, BinarySerilizationContext context)
		{
			Expression expression = member.GetProperty(context.MessageParameter);
			context.Code.Add(
				Expression.Assign(
					expression,
					Expression.Convert(
						Expression.Call(
							context.QueueParameter,
							MessageQueueMethods.ReadInt32,
							new Expression[] { Expression.Add(context.PositionParameter, Expression.Constant(member.Offset)) }),
						member.Type)));
		}

		public Expression BuildDynamicSizeEvaluator(MessageMemberInfo member, BinarySerilizationContext context)
		{
			return null;
		}

		public void BuildSerializeExpression(MessageMemberInfo member, BinarySerilizationContext context)
		{
			Expression expression = member.GetProperty(context.MessageParameter);
			if (member.Type != typeof(int))
			{
				expression = Expression.Convert(expression, typeof(int));
			}
			context.Code.Add(
				Expression.Call(
					context.QueueParameter,
					MessageQueueMethods.WriteInt32,
					Expression.Add(context.PositionParameter, Expression.Constant(member.Offset)),
					expression));
		}

		public bool CanHandleType(Type fieldType)
		{
			//if (typeof(int).IsAssignableFrom(fieldType)) return true;
			if (typeof(uint) == fieldType)
			{
				return true;
			}
			if (typeof(int) == fieldType)
			{
				return true;
			}
			if (typeof(ushort) == fieldType)
			{
				return true;
			}
			if (typeof(short) == fieldType)
			{
				return true;
			}
			if (typeof(byte) == fieldType)
			{
				return true;
			}
			if (typeof(sbyte) == fieldType)
			{
				return true;
			}
			return false;
		}

		#endregion
	}
}