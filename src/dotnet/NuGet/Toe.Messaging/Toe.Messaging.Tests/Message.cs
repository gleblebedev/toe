namespace Toe.Messaging.Tests
{
	public class Message
	{
	    private int messageId;

	    #region Public Properties

		[PropertyType(PropertyType.Int32)]
        [PropertyOrder(0)]
        public int MessageId
		{
		    get
		    {
		        return this.messageId;
		    }
		    set
		    {
		        this.messageId = value;
		    }
		}

	    #endregion
	}

    public class SubMessage : Message
    {
        public float B { get; set; }
        public int A { get; set; }
        public uint C { get; set; }
    }
}