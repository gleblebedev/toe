using System;
using System.Reflection;

using Toe.Messaging.Types;

namespace Toe.Messaging.Attributes
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
	public class PropertyTypeAttribute : Attribute
	{
		#region Constants and Fields

		private readonly int propertyType;

		#endregion

		#region Constructors and Destructors

		public PropertyTypeAttribute(int propertyType)
		{
			this.propertyType = propertyType;
		}

		#endregion

		#region Public Methods and Operators

		public static int Get(ParameterInfo parameterInfo, TypeRegistry typeRegistry = null)
		{
			var v = GetCustomAttribute(parameterInfo, typeof(PropertyTypeAttribute)) as PropertyTypeAttribute;
			if (v != null)
			{
				return v.propertyType;
			}
			return GetDefaultType(parameterInfo.ParameterType, typeRegistry);
		}

		public static int Get(MemberInfo property, TypeRegistry typeRegistry = null)
		{
			var v = GetCustomAttribute(property, typeof(PropertyTypeAttribute)) as PropertyTypeAttribute;
			if (v != null)
			{
				return v.propertyType;
			}
			var propertyInfo = property as PropertyInfo;
			if (propertyInfo != null)
			{
				return GetDefaultType(propertyInfo.PropertyType, typeRegistry);
			}
			var fieldInfo = property as FieldInfo;
			if (fieldInfo != null)
			{
				return GetDefaultType(fieldInfo.FieldType, typeRegistry);
			}
			return PropertyTypes.Unknown;
		}

		#endregion

		#region Methods

		private static int GetDefaultType(Type fieldType, TypeRegistry typeRegistry = null)
		{
			if (typeRegistry != null)
			{
				int res;
				if (typeRegistry.TryResolvePropertyType(fieldType, out res))
				{
					return res;
				}
			}
			if (fieldType == typeof(int))
			{
				return PropertyTypes.Int32;
			}
			if (fieldType == typeof(uint))
			{
				return PropertyTypes.Int32;
			}

			if (fieldType == typeof(byte))
			{
				return PropertyTypes.Int32;
			}
			if (fieldType == typeof(sbyte))
			{
				return PropertyTypes.Int32;
			}

			if (fieldType == typeof(ushort))
			{
				return PropertyTypes.Int32;
			}
			if (fieldType == typeof(short))
			{
				return PropertyTypes.Int32;
			}

			if (fieldType == typeof(float))
			{
				return PropertyTypes.Single;
			}
			if (fieldType == typeof(string))
			{
				return PropertyTypes.String;
			}
			return 0;
		}

		#endregion
	}
}