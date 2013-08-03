using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Toe.Messaging
{
	public class SignleBinarySerializer : ITypeBinarySerializer
	{
		#region Constants and Fields

		public static readonly ITypeBinarySerializer Instance = new SignleBinarySerializer();

		public static readonly MethodInfo writeFloat =
			((Func<int, float, BinarySerializerContext>)(new BinarySerializerContext()).WriteFloat).Method;

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

		#region Public Methods and Operators

		public Expression BuildDeserializeExpression(
			MessageMemberInfo member, Expression positionParameter, Expression queue, ParameterExpression messageParameter)
		{
			Expression expression = member.GetProperty(messageParameter);
			return Expression.Assign(
				expression,
				Expression.Convert(
					Expression.Call(
						queue, MessageQueueMethods.ReadFloat, Expression.Add(positionParameter, Expression.Constant(member.Offset))),
					member.PropertyType));
		}

		public Expression BuildDynamicSizeEvaluator(MessageMemberInfo member, ParameterExpression messageParameter)
		{
			return null;
		}

		public Expression BuildSerializeExpression(
			MessageMemberInfo member,
			Expression positionParameter,
			Expression dynamicPositionParameter,
			Expression queue,
			ParameterExpression messageParameter)
		{
			return Expression.Call(
				queue,
				MessageQueueMethods.WriteFloat,
				Expression.Add(positionParameter, Expression.Constant(member.Offset)),
				member.GetProperty(messageParameter));
		}

		#endregion
	}
}