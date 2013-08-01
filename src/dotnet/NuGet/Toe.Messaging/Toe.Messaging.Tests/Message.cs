namespace Toe.Messaging.Tests
{
	public class Message
	{
		#region Public Properties

		[MessagePropertyType(PropertyType.Int)]
		public int M { get; set; }

		#endregion
	}
}