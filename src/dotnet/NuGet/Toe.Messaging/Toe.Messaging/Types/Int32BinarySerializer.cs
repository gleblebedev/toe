using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Toe.Messaging
{
	public class Int32BinarySerializer : ITypeBinarySerializer
	{
		#region Constants and Fields

		public static readonly ITypeBinarySerializer Instance = new Int32BinarySerializer();

		public static readonly MethodInfo readInt32 =
			((Func<int, Func<int, int>, BinarySerializerContext>)(new BinarySerializerContext()).ReadInt32).Method;

		public static readonly MethodInfo writeInt32 =
			((Func<int, int, BinarySerializerContext>)(new BinarySerializerContext()).WriteInt32).Method;

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
						queue, MessageQueueMethods.ReadInt32, Expression.Add(positionParameter, Expression.Constant(member.Offset))),
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
			Expression expression = member.GetProperty(messageParameter);
			if (member.PropertyType != typeof(int))
			{
				expression = Expression.Convert(expression, typeof(int));
			}
			return Expression.Call(
				queue,
				MessageQueueMethods.WriteInt32,
				Expression.Add(positionParameter, Expression.Constant(member.Offset)),
				expression);
		}

		#endregion
	}
}