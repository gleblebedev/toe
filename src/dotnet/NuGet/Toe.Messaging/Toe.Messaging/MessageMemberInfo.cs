using System;
using System.Reflection;

namespace Toe.Messaging
{
    internal class MessageMemberInfo
    {
        private MemberInfo memberInfo;

        private int order;

        private string name;

        private PropertyType type;

        private Type propertyType;

        public MessageMemberInfo(PropertyInfo propertyInfo)
            : this((MemberInfo)propertyInfo)
        {
            this.propertyType = propertyInfo.PropertyType;
        }
        public MessageMemberInfo(FieldInfo fieldInfo)
            : this((MemberInfo)fieldInfo)
        {
            this.propertyType = (fieldInfo).FieldType;
        }
        public MessageMemberInfo(MemberInfo memberInfo)
        {
            this.memberInfo = memberInfo;
            this.order = PropertyOrderAttribute.Get(this.MemberInfo);
            this.name = PropertyNameAttribute.Get(this.MemberInfo);
            this.type = PropertyTypeAttribute.Get(this.MemberInfo);
        }

        public PropertyType Type
        {
            get
            {
                return this.type;
            }
        }

        public int Order
        {
            get
            {
                return this.order;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public int Offset { get; set; }

        public MemberInfo MemberInfo
        {
            get
            {
                return this.memberInfo;
            }
        }

        public Type PropertyType
        {
            get
            {
                return this.propertyType;
            }
        }
    }
}