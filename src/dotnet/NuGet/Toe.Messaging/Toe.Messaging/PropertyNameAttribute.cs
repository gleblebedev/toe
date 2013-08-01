using System;
using System.Reflection;

namespace Toe.Messaging
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
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

        public static string Get(MemberInfo property)
        {
            var v = GetCustomAttribute(property, typeof(PropertyNameAttribute)) as PropertyNameAttribute;
            if (v != null) return v.name;
            return property.Name;
        }
    }
}