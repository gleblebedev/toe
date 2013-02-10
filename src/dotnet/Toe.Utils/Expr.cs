using System;
using System.Linq.Expressions;

namespace Toe.Utils
{
	public static class Expr
	{
		#region Public Methods and Operators

		public static string Path<T>(Expression<Func<T, object>> tree)
		{
			return Path((MemberExpression)tree.Body);
		}

		#endregion

		#region Methods

		private static string Path(MemberExpression expression)
		{
			var memberExpression = expression.Expression as MemberExpression;
			if (memberExpression != null)
			{
				return Path(memberExpression) + "." + expression.Member.Name;
			}
			var parameterExpression = expression.Expression as ParameterExpression;
			if (parameterExpression != null)
			{
				return expression.Member.Name;
			}
			throw new ArgumentException(string.Format("Unsupported expression {0}", expression.Expression.GetType()));
		}

		#endregion
	}
}