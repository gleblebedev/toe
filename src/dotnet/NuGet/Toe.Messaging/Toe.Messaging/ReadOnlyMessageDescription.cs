using System;
using System.Collections.Generic;
using System.Linq;

namespace Toe.Messaging
{
	internal class ReadOnlyMessageDescription : IMessageDescription
	{
		#region Constants and Fields

		private readonly MessageDescription messageDescription;

		#endregion

		#region Constructors and Destructors

		public ReadOnlyMessageDescription(MessageDescription messageDescription)
		{
			this.messageDescription = messageDescription;
		}

		#endregion

		#region Public Properties

		public int MinSize
		{
			get
			{
				return this.messageDescription.MinSize;
			}
		}

		public IEnumerable<PropertyDescription> Properties
		{
			get
			{
				return this.messageDescription.Properties;
			}
		}

		#endregion

		#region Public Methods and Operators

		public void DefineProperty(string name, int propertyType, int offset, int size)
		{
			throw new NotImplementedException();
		}

		public PropertyDescription GetPropertyById(int propertyKey)
		{
			//TODO: make it fast!
			return this.messageDescription.Properties.FirstOrDefault(x => x.NameHash == propertyKey);
		}

		#endregion
	}
}