using System;
using System.Linq.Expressions;
using System.Reflection;

using Toe.Messaging.Attributes;
using Toe.Messaging.Types;

namespace Toe.Messaging
{
	public class MessageMemberInfo
	{
		#region Constants and Fields

		private readonly MemberInfo memberInfo;

		private readonly string name;

		private readonly int nameHash;

		private readonly int order;

		private readonly int propertyType;

		private readonly ITypeBinarySerializer serializer;

		private readonly Type type;

		#endregion

		#region Constructors and Destructors

		public MessageMemberInfo(PropertyInfo propertyInfo, TypeRegistry typeRegistry)
			: this((MemberInfo)propertyInfo, typeRegistry)
		{
			this.type = propertyInfo.PropertyType;
		}

		public MessageMemberInfo(FieldInfo fieldInfo, TypeRegistry typeRegistry)
			: this((MemberInfo)fieldInfo, typeRegistry)
		{
			this.type = (fieldInfo).FieldType;
		}

		public MessageMemberInfo(MemberInfo memberInfo, TypeRegistry typeRegistry)
		{
			this.memberInfo = memberInfo;
			this.order = PropertyOrderAttribute.Get(this.MemberInfo);
			this.name = PropertyNameAttribute.Get(this.MemberInfo);
			this.nameHash = Hash.Eval(this.name);
			this.propertyType = PropertyTypeAttribute.Get(this.MemberInfo);

			this.serializer = typeRegistry.ResolveSerializer(this.propertyType);
		}

		#endregion

		#region Public Properties

		public MemberInfo MemberInfo
		{
			get
			{
				return this.memberInfo;
			}
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public int NameHash
		{
			get
			{
				return this.nameHash;
			}
		}

		public int Offset { get; set; }

		public int Order
		{
			get
			{
				return this.order;
			}
		}

		public int PropertyType
		{
			get
			{
				return this.propertyType;
			}
		}

		public ITypeBinarySerializer Serializer
		{
			get
			{
				return this.serializer;
			}
		}

		public Type Type
		{
			get
			{
				return this.type;
			}
		}

		#endregion

		#region Public Methods and Operators

		public Expression GetProperty(Expression instance)
		{
			if (this.memberInfo is PropertyInfo)
			{
				return Expression.Property(instance, (PropertyInfo)this.memberInfo);
			}
			return Expression.Field(instance, (FieldInfo)this.memberInfo);
		}

		#endregion
	}
}