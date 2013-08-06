using System;
using System.Reflection;

namespace Toe.Messaging.Attributes
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
	public class PropertyNameAttribute : Attribute
	{
		#region Constants and Fields

		private readonly string name;

		#endregion

		#region Constructors and Destructors

		public PropertyNameAttribute(string name)
		{
			this.name = name;
		}

		#endregion

		#region Public Methods and Operators

		public static string Get(MemberInfo property)
		{
			var v = GetCustomAttribute(property, typeof(PropertyNameAttribute)) as PropertyNameAttribute;
			if (v != null)
			{
				return v.name;
			}
			return property.Name;
		}

		public static string Get(ParameterInfo parameterInfo)
		{
			var v = GetCustomAttribute(parameterInfo, typeof(PropertyNameAttribute)) as PropertyNameAttribute;
			if (v != null)
			{
				return v.name;
			}
			return parameterInfo.Name;
		}

		#endregion
	}
}