using System;
using System.Reflection;

namespace Toe.Messaging
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class PropertyTypeAttribute : Attribute
	{
		#region Constants and Fields

		private readonly PropertyType propertyType;

		#endregion

		#region Constructors and Destructors

		public PropertyTypeAttribute(PropertyType propertyType)
		{
			this.propertyType = propertyType;
		}

		#endregion

		public static PropertyType Get(MemberInfo property)
		{
			var v = GetCustomAttribute(property, typeof(PropertyTypeAttribute)) as PropertyTypeAttribute;
			if (v != null) return v.propertyType;
			return PropertyType.Unknown;
		}
	}
}