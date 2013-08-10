namespace Toe.Messaging.Tests
{
	public class SingleMessage<T> : Message
	{
		#region Public Properties

		public T A { get; set; }

		#endregion
	}
}