using System;
using System.Collections.Generic;

using Toe.Messaging.Attributes;

namespace Toe.Messaging
{
	/// <summary>
	/// Map of objects binded to messages
	/// </summary>
	public class ObjectMessageMap
	{
		#region Constants and Fields

		private readonly Dictionary<int, IMessageSerializer> map = new Dictionary<int, IMessageSerializer>();

		private readonly MessageRegistry registry;

		#endregion

		#region Constructors and Destructors

		public ObjectMessageMap(MessageRegistry registry)
		{
			this.registry = registry;
		}

		#endregion

		#region Public Methods and Operators

		public IMessageSerializer<T> GetSerializer<T>()
		{
			return this.GetSerializer<T>(MessageIdAttribute.Get(typeof(T)));
		}

		public IMessageSerializer<T> GetSerializer<T>(string messageName)
		{
			return this.GetSerializer<T>(Hash.Eval(messageName));
		}

		public IMessageSerializer<T> GetSerializer<T>(int messageId)
		{
			IMessageSerializer v;
			if (this.map.TryGetValue(messageId, out v))
			{
				return (IMessageSerializer<T>)v;
			}
			v = this.BuildFuncs<T>(typeof(T), messageId);
			this.map.Add(messageId, v);
			return (IMessageSerializer<T>)v;
		}

		#endregion

		#region Methods

		private IMessageSerializer BuildFuncs<T>(Type type, int messageId)
		{
			if (type == typeof(object))
			{
				return new DynamicSerializer(this.registry);
				;
			}
			return new DefaultSerializer<T>(this.registry, messageId);
		}

		#endregion
	}
}