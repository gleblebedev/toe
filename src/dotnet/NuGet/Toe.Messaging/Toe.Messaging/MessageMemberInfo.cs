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

		private readonly Type propertyType;

		private readonly ITypeBinarySerializer serializer;

		private readonly PropertyType type;

		#endregion

		#region Constructors and Destructors

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
			switch (this.type)
			{
				case Messaging.PropertyType.Int32:
					this.serializer = Int32BinarySerializer.Instance;
					break;
				case Messaging.PropertyType.Single:
					this.serializer = SignleBinarySerializer.Instance;
					break;
				case Messaging.PropertyType.String:
					this.serializer = StringBinarySerializer.Instance;
					break;
			}
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

		public Type PropertyType
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

		public PropertyType Type
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