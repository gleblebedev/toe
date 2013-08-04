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

		private readonly int order;

		private readonly Type type;

		private readonly ITypeBinarySerializer serializer;

		private readonly int propertyType;

		#endregion

		#region Constructors and Destructors

		public MessageMemberInfo(PropertyInfo propertyInfo)
			: this((MemberInfo)propertyInfo)
		{
			this.type = propertyInfo.PropertyType;
		}

		public MessageMemberInfo(FieldInfo fieldInfo)
			: this((MemberInfo)fieldInfo)
		{
			this.type = (fieldInfo).FieldType;
		}

		public MessageMemberInfo(MemberInfo memberInfo)
		{
			this.memberInfo = memberInfo;
			this.order = PropertyOrderAttribute.Get(this.MemberInfo);
			this.name = PropertyNameAttribute.Get(this.MemberInfo);
			this.propertyType = PropertyTypeAttribute.Get(this.MemberInfo);
			if (this.propertyType == Messaging.PropertyType.Int32)
				this.serializer = Int32BinarySerializer.Instance;
			else if (this.propertyType == Messaging.PropertyType.Single)
				this.serializer = SignleBinarySerializer.Instance;
			else if (this.propertyType == Messaging.PropertyType.String)
				this.serializer = StringBinarySerializer.Instance;
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

		public int Offset { get; set; }

		public int Order
		{
			get
			{
				return this.order;
			}
		}

		public Type Type
		{
			get
			{
				return this.type;
			}
		}

		public ITypeBinarySerializer Serializer
		{
			get
			{
				return this.serializer;
			}
		}

		public int PropertyType
		{
			get
			{
				return this.propertyType;
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