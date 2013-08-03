using System.Linq.Expressions;

namespace Toe.Messaging.Types
{
	public interface ITypeBinarySerializer
	{
		#region Public Properties

		int FixedSize { get; }

		#endregion

		#region Public Methods and Operators

		Expression BuildDeserializeExpression(
			MessageMemberInfo member, Expression positionParameter, Expression queue, ParameterExpression messageParameter);

		Expression BuildDynamicSizeEvaluator(MessageMemberInfo member, ParameterExpression messageParameter);

		Expression BuildSerializeExpression(
			MessageMemberInfo member,
			Expression positionParameter,
			Expression dynamicPositionParameter,
			Expression queue,
			ParameterExpression messageParameter);

		#endregion
	}
}