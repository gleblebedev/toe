using System;

using Toe.Messaging.Attributes;

namespace Toe.Messaging
{
	[CLSCompliant(true)]
	public class Message
	{
		#region Public Properties

		//[PropertyType(PropertyType.Int32)]
		[PropertyOrder(int.MinValue)]
		public int MessageId { get; set; }

		#endregion
	}
}