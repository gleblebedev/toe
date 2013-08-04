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

		#endregion

		public void BuildDeserializeExpression(MessageMemberInfo member, BinarySerilizationContext context)
		{
			Expression expression = member.GetProperty(context.MessageParameter);
			context.Code.Add(Expression.Assign(
				expression,
				Expression.Convert(
					Expression.Call(
						context.QueueParameter,
						MessageQueueMethods.ReadInt32,
						new Expression[] { Expression.Add(context.PositionParameter, Expression.Constant(member.Offset)) }),
					member.PropertyType)));
		}

		public Expression BuildDynamicSizeEvaluator(MessageMemberInfo member, BinarySerilizationContext context)
		{
			return null;
		}

		public void BuildSerializeExpression(MessageMemberInfo member, BinarySerilizationContext context)
		{
			Expression expression = member.GetProperty(context.MessageParameter);
			if (member.PropertyType != typeof(int))
			{
				expression = Expression.Convert(expression, typeof(int));
			}
			context.Code.Add( Expression.Call(
				context.QueueParameter,
				MessageQueueMethods.WriteInt32,
				Expression.Add(context.PositionParameter, Expression.Constant(member.Offset)),
				expression));

		}


		#region Public Methods and Operators


		#endregion
	}
}