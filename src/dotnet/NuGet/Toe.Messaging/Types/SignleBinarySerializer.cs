using System;
using System.Linq.Expressions;

namespace Toe.Messaging.Types
{
	public class SignleBinarySerializer : ITypeBinarySerializer
	{
		#region Constants and Fields

		public static readonly ITypeBinarySerializer Instance = new SignleBinarySerializer();

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
				return PropertyTypes.Single;
			}
		}

		#endregion

		#region Public Methods and Operators

		public Expression BuildDeserializeExpression(MessageMemberInfo member, BinarySerializationContext context)
		{
			return
				Expression.Convert(
					Expression.Call(
						context.QueueParameter,
						MessageQueueMethods.ReadFloat,
						Expression.Add(context.PositionParameter, Expression.Constant(member.Offset))),
					member.Type);
		}

		public Expression BuildDynamicSizeEvaluator(MessageMemberInfo member, BinarySerializationContext context)
		{
			return null;
		}

		public void BuildSerializeExpression(MessageMemberInfo member, BinarySerializationContext context)
		{
			context.Code.Add(
				Expression.Call(
					context.QueueParameter,
					MessageQueueMethods.WriteFloat,
					Expression.Add(context.PositionParameter, Expression.Constant(member.Offset)),
					member.GetProperty(context.MessageParameter)));
		}

		public bool CanHandleType(Type fieldType)
		{
			return typeof(float) == fieldType;
		}

		#endregion
	}
}