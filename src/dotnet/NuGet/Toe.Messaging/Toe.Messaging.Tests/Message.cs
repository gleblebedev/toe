using Toe.Messaging.Attributes;

namespace Toe.Messaging.Tests
{
	public class Message
	{
		#region Public Properties

		[PropertyType(PropertyType.Int32)]
		[PropertyOrder(0)]
		public int MessageId { get; set; }

		#endregion
	}

	public class SubMessage : Message
	{
		public float B { get; set; }

		#region Public Properties

		public int A { get; set; }

		public uint C { get; set; }

		public string Text { get; set; }

		#endregion
	}
}