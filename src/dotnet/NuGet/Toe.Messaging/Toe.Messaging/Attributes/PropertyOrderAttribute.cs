using System;
using System.Reflection;

namespace Toe.Messaging.Attributes
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class PropertyOrderAttribute : Attribute
	{
		#region Constants and Fields

		private readonly int order;

		#endregion

		#region Constructors and Destructors

		public PropertyOrderAttribute(int order)
		{
			this.order = order;
		}

		#endregion

		#region Public Methods and Operators

		public static int Get(MemberInfo property)
		{
			var v = GetCustomAttribute(property, typeof(PropertyOrderAttribute)) as PropertyOrderAttribute;
			if (v != null)
			{
				return v.order;
			}
			return int.MaxValue;
		}

		#endregion
	}
}