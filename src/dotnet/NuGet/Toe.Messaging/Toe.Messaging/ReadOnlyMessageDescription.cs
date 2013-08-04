using System;
using System.Collections.Generic;
using System.Linq;

namespace Toe.Messaging
{
	internal class ReadOnlyMessageDescription : IMessageDescription
	{
		private readonly MessageDescription messageDescription;

		public ReadOnlyMessageDescription(MessageDescription messageDescription)
		{
			this.messageDescription = messageDescription;
		}

		#region Implementation of IMessageDescription

		public IEnumerable<PropertyDescription> Properties { get
		{
			return messageDescription.Properties;
		} }

		public int MinSize { get
		{
			return messageDescription.MinSize;
		} }

		public void DefineProperty(string name, int propertyType, int offset, int size)
		{
			throw new NotImplementedException();
		}

		public PropertyDescription GetPropertyById(int propertyKey)
		{
			//TODO: make it fast!
			return messageDescription.Properties.FirstOrDefault(x => x.NameHash == propertyKey);
		}

		#endregion
	}
}