namespace Toe.Messaging
{
	internal class NewMessageDescription : IMessageDescription
	{
		#region Constants and Fields

		private readonly MessageDescription messageDescription;

		private readonly IMessageDescription parentMessage;

		#endregion

		#region Constructors and Destructors

		public NewMessageDescription(MessageDescription messageDescription, IMessageDescription parentMessage)
		{
			this.messageDescription = messageDescription;
			this.parentMessage = parentMessage;
		}

		#endregion
	}
}