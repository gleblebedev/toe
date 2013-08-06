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


		private readonly string name;

		private readonly int nameHash;

		private readonly int order;

		private readonly int propertyType;

		private readonly ITypeBinarySerializer serializer;

		private readonly Type type;

		private object memberInfo;

		#endregion

		#region Constructors and Destructors

		public MessageMemberInfo(PropertyInfo propertyInfo, TypeRegistry typeRegistry)
			: this((MemberInfo)propertyInfo, typeRegistry)
		{
			this.type = propertyInfo.PropertyType;
			this.memberInfo = propertyInfo;
		}

		public MessageMemberInfo(FieldInfo fieldInfo, TypeRegistry typeRegistry)
			: this((MemberInfo)fieldInfo, typeRegistry)
		{
			this.type = (fieldInfo).FieldType;
			this.memberInfo = fieldInfo;
		}

		public MessageMemberInfo(MemberInfo memberInfo, TypeRegistry typeRegistry)
		{
	
			this.order = PropertyOrderAttribute.Get(memberInfo);
			this.name = PropertyNameAttribute.Get(memberInfo);
			this.nameHash = Hash.Eval(this.name);
			this.propertyType = PropertyTypeAttribute.Get(memberInfo, typeRegistry);

			this.serializer = typeRegistry.ResolveSerializer(this.propertyType);
		}

		public MessageMemberInfo(ParameterInfo par, TypeRegistry typeRegistry)
		{
			this.memberInfo = par;
			this.type = par.ParameterType;

			this.order = PropertyOrderAttribute.Get(par);
			this.name = PropertyNameAttribute.Get(par);
			this.nameHash = Hash.Eval(this.name);
			this.propertyType = PropertyTypeAttribute.Get(par, typeRegistry);
			this.serializer = typeRegistry.ResolveSerializer(this.propertyType);
		}



		#endregion

		#region Public Properties


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
			var propertyInfo = this.memberInfo as PropertyInfo;
			if (propertyInfo != null)
			{
				return Expression.Property(instance, propertyInfo);
			}
			return Expression.Field(instance, (FieldInfo)this.memberInfo);
		}

		#endregion
	}
}