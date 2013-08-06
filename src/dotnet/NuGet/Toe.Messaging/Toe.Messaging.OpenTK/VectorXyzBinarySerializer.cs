using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using OpenTK;

using Toe.Messaging.Types;

namespace Toe.Messaging.OpenTK
{
	public class VectorXyzBinarySerializer : ITypeBinarySerializer
	{
		#region Constants and Fields

		public static readonly ITypeBinarySerializer Instance = new SignleBinarySerializer();

		#endregion

		#region Public Properties

		public int FixedSize
		{
			get
			{
				return 3;
			}
		}

		public int PropertyType
		{
			get
			{
				return Messaging.PropertyType.VectorXYZ;
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
					Expression.New(
						typeof(Vector3).GetConstructor(new[] { typeof(float), typeof(float), typeof(float) }),
						new Expression[]
							{
								Expression.Call(
									context.QueueParameter,
									MessageQueueMethods.ReadFloat,
									Expression.Add(context.PositionParameter, Expression.Constant(member.Offset))),
								Expression.Call(
									context.QueueParameter,
									MessageQueueMethods.ReadFloat,
									Expression.Add(context.PositionParameter, Expression.Constant(member.Offset + 1))),
								Expression.Call(
									context.QueueParameter,
									MessageQueueMethods.ReadFloat,
									Expression.Add(context.PositionParameter, Expression.Constant(member.Offset + 2))),
							})));
		}

		public Expression BuildDynamicSizeEvaluator(MessageMemberInfo member, BinarySerilizationContext context)
		{
			return null;
		}

		public void BuildSerializeExpression(MessageMemberInfo member, BinarySerilizationContext context)
		{
			context.Code.Add(
				Expression.Call(
					context.QueueParameter,
					MessageQueueMethods.WriteFloat,
					Expression.Add(context.PositionParameter, Expression.Constant(member.Offset)),
					Expression.Field(member.GetProperty(context.MessageParameter), typeof(Vector3).GetField("X"))));
			context.Code.Add(
				Expression.Call(
					context.QueueParameter,
					MessageQueueMethods.WriteFloat,
					Expression.Add(context.PositionParameter, Expression.Constant(member.Offset + 1)),
					Expression.Field(member.GetProperty(context.MessageParameter), typeof(Vector3).GetField("Y"))));
			context.Code.Add(
				Expression.Call(
					context.QueueParameter,
					MessageQueueMethods.WriteFloat,
					Expression.Add(context.PositionParameter, Expression.Constant(member.Offset + 2)),
					Expression.Field(member.GetProperty(context.MessageParameter), typeof(Vector3).GetField("Z"))));
		}

		public bool CanHandleType(Type fieldType)
		{
			return typeof(Vector3) == fieldType;
		}

		#endregion
	}
}
