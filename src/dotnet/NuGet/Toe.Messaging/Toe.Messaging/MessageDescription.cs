using System.Collections.Generic;

namespace Toe.Messaging
{
	internal class MessageDescription
	{
		#region Constants and Fields

		public int MessageId;

		public int ParentId;

		private readonly List<PropertyDescription> properties = new List<PropertyDescription>(8);

		#endregion

		#region Public Properties

		public List<PropertyDescription> Properties
		{
			get
			{
				return this.properties;
			}
		}

		public IMessageDescription ReadOnlyDescription { get; set; }

		public int MinSize { get; set; }

		#endregion
	}
}