using System;
using System.Collections.Generic;
using System.Linq;

namespace Toe.Messaging
{
	internal class WritableMessageDescription : IMessageDescription
	{
		#region Constants and Fields

		private readonly MessageDescription messageDescription;

		private readonly IMessageDescription parentMessage;

		private int propertyIndex;

		#endregion

		#region Constructors and Destructors

		public WritableMessageDescription(MessageDescription messageDescription, IMessageDescription parentMessage)
		{
			this.messageDescription = messageDescription;
			this.parentMessage = parentMessage;
		}

		#endregion

		#region Public Properties

		public IEnumerable<PropertyDescription> Properties
		{
			get
			{
				return this.messageDescription.Properties;
			}
		}

		public int MinSize { get
		{
			return messageDescription.MinSize;
		} }

		#endregion

		#region Public Methods and Operators

		public void DefineProperty(string name, int propertyType, int offset, int size)
		{
			var propertyDescription = new PropertyDescription(name, offset, size, propertyType);
			if (this.propertyIndex < this.messageDescription.Properties.Count)
			{
				if (this.messageDescription.Properties[this.propertyIndex] != propertyDescription)
				{
					throw new MessageRegistryException(name);
				}
				++this.propertyIndex;
				return;
			}
			this.messageDescription.Properties.Add(propertyDescription);
			++this.propertyIndex;
		}

		public PropertyDescription GetPropertyById(int propertyKey)
		{
			return messageDescription.Properties.FirstOrDefault(x => x.NameHash == propertyKey);
		}

		#endregion
	}
}