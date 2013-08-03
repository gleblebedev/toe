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

		#region Public Methods and Operators

		public static PropertyType Get(MemberInfo property)
		{
			var v = GetCustomAttribute(property, typeof(PropertyTypeAttribute)) as PropertyTypeAttribute;
			if (v != null)
			{
				return v.propertyType;
			}
			var propertyInfo = property as PropertyInfo;
			if (propertyInfo != null)
			{
				return GetDefaultType(propertyInfo.PropertyType);
			}
			var fieldInfo = property as FieldInfo;
			if (fieldInfo != null)
			{
				return GetDefaultType(fieldInfo.FieldType);
			}
			return PropertyType.Unknown;
		}

		#endregion

		#region Methods

		private static PropertyType GetDefaultType(Type fieldType)
		{
			if (fieldType == typeof(int))
			{
				return PropertyType.Int32;
			}
			if (fieldType == typeof(uint))
			{
				return PropertyType.Int32;
			}

			if (fieldType == typeof(byte))
			{
				return PropertyType.Int32;
			}
			if (fieldType == typeof(sbyte))
			{
				return PropertyType.Int32;
			}

			if (fieldType == typeof(ushort))
			{
				return PropertyType.Int32;
			}
			if (fieldType == typeof(short))
			{
				return PropertyType.Int32;
			}

			if (fieldType == typeof(float))
			{
				return PropertyType.Single;
			}
			if (fieldType == typeof(string))
			{
				return PropertyType.String;
			}
			return PropertyType.Unknown;
		}

		#endregion
	}
}