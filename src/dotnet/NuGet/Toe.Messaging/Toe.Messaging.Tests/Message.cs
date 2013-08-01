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
        public string Text { get; set; }
    }
}