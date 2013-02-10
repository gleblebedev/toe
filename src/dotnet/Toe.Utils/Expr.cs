using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Toe.Utils
{
	public struct PropertyEventArgs
	{
		private readonly PropertyChangingEventArgs changing;

		private readonly PropertyChangedEventArgs changed;

		public PropertyEventArgs(PropertyChangingEventArgs changing,PropertyChangedEventArgs changed)
		{
			this.changing = changing;
			this.changed = changed;
		}

		public PropertyEventArgs(string propertyName)
		{
			this.changing = new PropertyChangingEventArgs(propertyName);
			this.changed = new PropertyChangedEventArgs(propertyName);
		}

		public PropertyChangingEventArgs Changing
		{
			get
			{
				return this.changing;
			}
		}

		public PropertyChangedEventArgs Changed
		{
			get
			{
				return this.changed;
			}
		}
	}
	public static class Expr
	{
		#region Public Methods and Operators
		public static PropertyChangedEventArgs PropChanged<T>(Expression<Func<T, object>> tree)
		{
			return new PropertyChangedEventArgs(Path(tree));
		}
		public static PropertyChangingEventArgs PropChanging<T>(Expression<Func<T, object>> tree)
		{
			return new PropertyChangingEventArgs(Path(tree));
		}

		public static PropertyEventArgs PropertyEventArgs<T>(Expression<Func<T, object>> tree)
		{
			var propertyName = Path(tree);
			return new PropertyEventArgs(propertyName);
		}

		public static string Path<T>(Expression<Func<T, object>> tree)
		{
			var memberExpression = tree.Body as MemberExpression;
			if (memberExpression != null)
				return Path(memberExpression);
			var unaryExpression = tree.Body as UnaryExpression;
			if (unaryExpression != null)
			{
				memberExpression = unaryExpression.Operand as MemberExpression;
				if (memberExpression != null)
					return Path(memberExpression);
			}
			throw new ArgumentException(string.Format("Unsupported expression {0}", tree.Body.GetType()));
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