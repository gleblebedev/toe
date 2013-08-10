using System;

using Toe.Messaging.Attributes;

namespace Toe.Messaging
{
	/// <summary>
	/// Base message container.
	/// </summary>
	[CLSCompliant(true)]
	public class Message
	{
		#region Public Properties

		/// <summary>
		/// Message type id.
		/// The value should be equal to the Hash.Eval function result of message name.
		/// </summary>
		[PropertyType(PropertyTypes.Int32)]
		[PropertyOrder(int.MinValue)]
		public int MessageId { get; set; }

		#endregion
	}
}