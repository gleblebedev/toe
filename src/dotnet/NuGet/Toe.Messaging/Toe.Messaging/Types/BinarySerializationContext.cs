using System.Collections.Generic;
using System.Linq.Expressions;

namespace Toe.Messaging.Types
{
	public class BinarySerializationContext
	{
		#region Constants and Fields

		private readonly List<Expression> code = new List<Expression>();

		private readonly ParameterExpression dynamicPositionParameter;

		private readonly List<ParameterExpression> localVariables = new List<ParameterExpression>();

		private readonly ParameterExpression messageParameter;

		private readonly ParameterExpression positionParameter;

		private readonly ParameterExpression queueParameter;

		#endregion

		#region Constructors and Destructors

		public BinarySerializationContext(
			ParameterExpression queueParameter,
			ParameterExpression messageParameter,
			ParameterExpression positionParameter,
			ParameterExpression dynamicPositionParameter)
		{
			this.queueParameter = queueParameter;
			this.messageParameter = messageParameter;
			this.positionParameter = positionParameter;
			this.dynamicPositionParameter = dynamicPositionParameter;
		}

		#endregion

		#region Public Properties

		public List<Expression> Code
		{
			get
			{
				return this.code;
			}
		}

		public ParameterExpression DynamicPositionParameter
		{
			get
			{
				return this.dynamicPositionParameter;
			}
		}

		public IList<ParameterExpression> LocalVariables
		{
			get
			{
				return this.localVariables;
			}
		}

		public ParameterExpression MessageParameter
		{
			get
			{
				return this.messageParameter;
			}
		}

		public ParameterExpression PositionParameter
		{
			get
			{
				return this.positionParameter;
			}
		}

		public ParameterExpression QueueParameter
		{
			get
			{
				return this.queueParameter;
			}
		}

		#endregion
	}
}