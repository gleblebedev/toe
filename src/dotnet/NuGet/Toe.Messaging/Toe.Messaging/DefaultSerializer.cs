using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Toe.CircularArrayQueue;

namespace Toe.Messaging
{
	public class DefaultSerializer<T> : IMessageSerializer<T>
	{
		private readonly int messageId;
		
		internal class PropertyBinding
		{
			public int PropertyId;
			public PropertyType Type;

			public int Size;
			public int Offset;

			public Expression Setter;
			public Expression Getter;
			public Expression Allocation;

			
		}
		private PropertyBinding GetPropertyBinding(PropertyInfo property)
		{
			var type = MessagePropertyTypeAttribute.Get(property);
			if (type == PropertyType.Unknown) return null;
			return new PropertyBinding() {  PropertyId = Hash.Eval(property.Name), Type = type };
		}
		public DefaultSerializer(MessageRegistry messageRegistry, int messageId)
		{
			this.messageId = messageId;
			messageRegistry.DefineMessage(this.messageId);

			var parameter = Expression.Parameter(typeof(T), "value");
			
			var all = from p in typeof(T).GetProperties(BindingFlags.Public) let binding = GetPropertyBinding(p) where binding != null select binding;
		}

		#region Implementation of IMessageSerializer

		int IMessageSerializer.Serialize(IMessageQueue queue, object value)
		{
			return this.Serialize(queue,(T)value);
		}

		void IMessageSerializer.Deserialize(IMessageQueue queue, int pos, object value)
		{
			this.Deserialize(queue, pos, (T)value);
		}

		#endregion

		#region Implementation of IMessageSerializer<T>

		public int Serialize(IMessageQueue queue, T value)
		{
			throw new NotImplementedException();
		}

		public void Deserialize(IMessageQueue queue,int pos, T value)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}