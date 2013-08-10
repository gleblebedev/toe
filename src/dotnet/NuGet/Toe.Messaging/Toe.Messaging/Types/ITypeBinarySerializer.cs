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

		Expression BuildDeserializeExpression(MessageMemberInfo member, BinarySerializationContext context);

		Expression BuildDynamicSizeEvaluator(MessageMemberInfo member, BinarySerializationContext context);

		void BuildSerializeExpression(MessageMemberInfo member, BinarySerializationContext context);

		bool CanHandleType(Type fieldType);

		#endregion
	}
}