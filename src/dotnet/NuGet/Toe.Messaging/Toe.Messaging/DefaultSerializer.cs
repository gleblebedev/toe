using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Toe.CircularArrayQueue;
using System.Threading;

namespace Toe.Messaging
{
	public class DefaultSerializer<T> : IMessageSerializer<T>
	{
		private readonly int messageId;

		private BinarySerializerContext reusableContext = new BinarySerializerContext();

		private Action<IMessageQueue, T> serialize;

		private void ReleaseContext(BinarySerializerContext context)
		{
			Interlocked.CompareExchange(ref reusableContext, context, null);
		}
		private BinarySerializerContext AllocateContext()
		{
			BinarySerializerContext context;
			do
			{
				context = reusableContext;
				if (context == null) return new BinarySerializerContext();
			}
			while (context != Interlocked.CompareExchange(ref reusableContext, null, context));
			return context;
		}
		
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
			var type = PropertyTypeAttribute.Get(property);
			if (type == PropertyType.Unknown) return null;
			return new PropertyBinding() {  PropertyId = Hash.Eval(property.Name), Type = type };
		}
		public DefaultSerializer(MessageRegistry messageRegistry, int messageId)
		{
			this.messageId = messageId;
			messageRegistry.DefineMessage(this.messageId);

			var parameter = Expression.Parameter(typeof(T), "value");

			var all = (from p in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly) let binding = GetPropertyBinding(p) where binding != null select binding).ToList();
			var fixedSize = 0;
			var allocateContext = ((Func<BinarySerializerContext>) AllocateContext).Method;
			var releaseContext = ((Action<BinarySerializerContext>) ReleaseContext).Method;
			var allocate = ((Func<IMessageQueue,int,int,BinarySerializerContext>)reusableContext.Allocate).Method;
			var commit = ((Func<BinarySerializerContext>)reusableContext.Commit).Method;
			var queueParameter = Expression.Parameter(typeof(IMessageQueue), "q");
			var messageParameter = Expression.Parameter(typeof(T), "m");
			var thisExpression = Expression.Constant(this);
			var chainElement = Expression.Call(thisExpression, allocateContext);
			chainElement = Expression.Call(
				chainElement, allocate, queueParameter, Expression.Constant(fixedSize), Expression.Constant(0));
			chainElement = Expression.Call(chainElement, commit);
			var body = Expression.Call(thisExpression, releaseContext, chainElement);
			var serializeExpression = Expression.Lambda<Action<IMessageQueue, T>>(body, queueParameter, messageParameter);
			this.serialize = serializeExpression.Compile();
		}

		#region Implementation of IMessageSerializer

		void IMessageSerializer.Serialize(IMessageQueue queue, object value)
		{
			this.Serialize(queue,(T)value);
		}

		void IMessageSerializer.Deserialize(IMessageQueue queue, int pos, object value)
		{
			this.Deserialize(queue, pos, (T)value);
		}

		#endregion

		#region Implementation of IMessageSerializer<T>

		public void Serialize(IMessageQueue queue, T value)
		{
			serialize(queue, value);
		}

		public void Deserialize(IMessageQueue queue,int pos, T value)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
