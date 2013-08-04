using System.Linq.Expressions;

namespace Toe.Messaging.Types
{
	public interface ITypeBinarySerializer
	{
		#region Public Properties

		int FixedSize { get; }

		#endregion

		#region Public Methods and Operators

		void BuildDeserializeExpression(MessageMemberInfo member, BinarySerilizationContext context);

		Expression BuildDynamicSizeEvaluator(MessageMemberInfo member, BinarySerilizationContext context);

		void BuildSerializeExpression(MessageMemberInfo member,BinarySerilizationContext context);

		#endregion
	}
}