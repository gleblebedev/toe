using System.Collections.Generic;
using System.Linq.Expressions;

namespace Toe.Messaging.Types
{
	public class BinarySerilizationContext
	{
		private List<ParameterExpression> localVariables = new List<ParameterExpression>();

		private ParameterExpression messageParameter;

		private ParameterExpression queueParameter;

		private ParameterExpression positionParameter;

		private ParameterExpression dynamicPositionParameter;

		public IList<ParameterExpression> LocalVariables
		{
			get
			{
				return this.localVariables;
			}
		}

		public List<Expression> Code
		{
			get
			{
				return this.code;
			}
		}

		public ParameterExpression MessageParameter
		{
			get
			{
				return this.messageParameter;
			}
		}

		public ParameterExpression QueueParameter
		{
			get
			{
				return this.queueParameter;
			}
			set
			{
				this.queueParameter = value;
			}
		}

		public ParameterExpression PositionParameter
		{
			get
			{
				return this.positionParameter;
			}
			set
			{
				this.positionParameter = value;
			}
		}

		public ParameterExpression DynamicPositionParameter
		{
			get
			{
				return this.dynamicPositionParameter;
			}
			set
			{
				this.dynamicPositionParameter = value;
			}
		}

		private List<Expression> code = new List<Expression>();

		public BinarySerilizationContext(ParameterExpression queueParameter, ParameterExpression messageParameter, ParameterExpression positionParameter, ParameterExpression dynamicPositionParameter)
		{
			this.queueParameter = queueParameter;
			this.messageParameter = messageParameter;
			this.positionParameter = positionParameter;
			this.dynamicPositionParameter = dynamicPositionParameter;
		}
	}
}