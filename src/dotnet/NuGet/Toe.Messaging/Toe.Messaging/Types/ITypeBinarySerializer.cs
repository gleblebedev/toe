using System;
using System.Linq.Expressions;

namespace Toe.Messaging.Types
{
	public interface ITypeBinarySerializer
	{
		#region Public Properties

		int FixedSize { get; }

		int PropertyType { get; }

		#endregion

		#region Public Methods and Operators

		Expression BuildDeserializeExpression(MessageMemberInfo member, BinarySerilizationContext context);

		Expression BuildDynamicSizeEvaluator(MessageMemberInfo member, BinarySerilizationContext context);

		void BuildSerializeExpression(MessageMemberInfo member, BinarySerilizationContext context);

		bool CanHandleType(Type fieldType);

		#endregion
	}
}