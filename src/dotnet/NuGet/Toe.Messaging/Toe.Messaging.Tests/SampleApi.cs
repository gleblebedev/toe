using System;

using Toe.Messaging.Attributes;

namespace Toe.Messaging.Tests
{
	/// <summary>
	/// Base message container.
	/// </summary>
	[CLSCompliant(true)]
	public class SampleApi
	{
		[MessageId("SubMessage")]
		public void OnSubMessage(
			[PropertyOrder(int.MinValue)] int MessageId, int A, float B, uint C, byte Byte, string Text)
		{
			
		}
	}
}