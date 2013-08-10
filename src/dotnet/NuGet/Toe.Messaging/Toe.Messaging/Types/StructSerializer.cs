using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Toe.Messaging.Types
{
	public class StructSerializer<T> : ITypeBinarySerializer
	{
		#region Constants and Fields

		private readonly IEnumerable<MemberExpression> constructorArgs;

		private readonly IEnumerable<MemberExpression> seriliationOrder;

		private readonly int typeId;

		private int fixedSize;

		#endregion

		#region Constructors and Destructors

		public StructSerializer(int typeId, params Expression<Func<T, object>>[] constructorArgs)
			: this(typeId, constructorArgs, constructorArgs)
		{
		}

		public StructSerializer(
			int typeId,
			IEnumerable<Expression<Func<T, object>>> constructorArgs,
			IEnumerable<Expression<Func<T, object>>> seriliationOrder)
		{
			this.typeId = typeId;
			this.constructorArgs = from a in constructorArgs select (MemberExpression)((UnaryExpression)a.Body).Operand;
			this.seriliationOrder = from a in constructorArgs select (MemberExpression)((UnaryExpression)a.Body).Operand;
			this.fixedSize = 0;
			foreach (var expression in this.constructorArgs)
			{
				this.fixedSize += this.GetFixedSizeOf(expression);
			}
		}

		#endregion

		#region Public Properties

		public int FixedSize
		{
			get
			{
				return this.fixedSize;
			}
			set
			{
				this.fixedSize = value;
			}
		}

		public int PropertyType
		{
			get
			{
				return this.typeId;
			}
		}

		#endregion

		#region Public Methods and Operators

		public Expression BuildDeserializeExpression(MessageMemberInfo member, BinarySerializationContext context)
		{
			List<Type> constructorArgTypes = new List<Type>();
			List<Expression> constructorArgValues = new List<Expression>();

			foreach (MemberExpression operand in this.constructorArgs)
			{
				MethodInfo methodInfo;
				var t = operand.Type;
				Type convertTo = null;
				if (typeof(int) == t)
				{
					methodInfo = MessageQueueMethods.ReadInt32;
				}
				else if (typeof(uint) == t)
				{
					methodInfo = MessageQueueMethods.ReadInt32;
					convertTo = typeof(int);
				}
				else if (typeof(float) == t)
				{
					methodInfo = MessageQueueMethods.ReadFloat;
				}
				else
				{
					throw new NotImplementedException();
				}

				constructorArgTypes.Add(t);
				Expression methodCallExpression = Expression.Call(
					context.QueueParameter,
					methodInfo,
					Expression.Add(context.PositionParameter, Expression.Constant(this.GetOffset(operand) + member.Offset)));
				if (convertTo != null)
				{
					methodCallExpression = Expression.Convert(methodCallExpression, convertTo);
				}
				constructorArgValues.Add(methodCallExpression);
			}

			return Expression.New(typeof(T).GetConstructor(constructorArgTypes.ToArray()), constructorArgValues);
		}

		public Expression BuildDynamicSizeEvaluator(MessageMemberInfo member, BinarySerializationContext context)
		{
			return null;
		}

		public void BuildSerializeExpression(MessageMemberInfo member, BinarySerializationContext context)
		{
			int i = 0;
			foreach (var expression in this.seriliationOrder)
			{
				MethodInfo methodInfo;
				var t = expression.Type;
				Type convertTo = null;
				if (typeof(int) == t)
				{
					methodInfo = MessageQueueMethods.WriteInt32;
				}
				else if (typeof(uint) == t)
				{
					methodInfo = MessageQueueMethods.WriteInt32;
					convertTo = typeof(int);
				}
				else if (typeof(float) == t)
				{
					methodInfo = MessageQueueMethods.WriteFloat;
				}
				else
				{
					throw new NotImplementedException();
				}

				Expression memberExpression = (expression.Member is FieldInfo)
					                              ? Expression.Field(
						                              member.GetProperty(context.MessageParameter), (FieldInfo)expression.Member)
					                              : Expression.Property(
						                              member.GetProperty(context.MessageParameter), (PropertyInfo)expression.Member);
				if (convertTo != null)
				{
					memberExpression = Expression.Convert(memberExpression, convertTo);
				}
				context.Code.Add(
					Expression.Call(
						context.QueueParameter,
						methodInfo,
						Expression.Add(context.PositionParameter, Expression.Constant(i + member.Offset)),
						memberExpression));
				++i;
			}
		}

		public bool CanHandleType(Type fieldType)
		{
			return (fieldType == typeof(T));
		}

		#endregion

		#region Methods

		private int GetFixedSizeOf(MemberExpression expression)
		{
			return 1;
		}

		private int GetOffset(MemberExpression operand)
		{
			int i = 0;
			foreach (var memberExpression in this.seriliationOrder)
			{
				if (memberExpression.Member == operand.Member)
				{
					return i;
				}
				++i;
			}
			throw new InvalidOperationException();
		}

		#endregion
	}
}