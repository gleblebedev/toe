using System;
using System.Reflection;

namespace Toe.Messaging
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class MessagePropertyTypeAttribute : Attribute
	{
		#region Constants and Fields

		private readonly PropertyType propertyType;

		#endregion

		#region Constructors and Destructors

		public MessagePropertyTypeAttribute(PropertyType propertyType)
		{
			this.propertyType = propertyType;
		}

		#endregion

		public static PropertyType Get(MemberInfo property)
		{
			var v = GetCustomAttribute(property, typeof(MessagePropertyTypeAttribute)) as MessagePropertyTypeAttribute;
			if (v != null) return v.propertyType;
			return PropertyType.Unknown;
		}
	}
}