using NUnit.Framework;

using Toe.CircularArrayQueue;
using Toe.Messaging.Attributes;
using Toe.Messaging.Types;

namespace Toe.Messaging.Tests
{
	[TestFixture]
	public class TestMessageDispatcher
	{
		public class SampleMessage : Message
		{
			public int A { get; set; }
		}
		public class SampleApiClass
		{
			public bool OnSampleMessage([PropertyOrder(int.MinValue), PropertyName("MessageId")] int id, [PropertyName("A")] int a)
			{
				Assert.AreEqual(10,a);
				return true;
			}
		}
		#region Constants and Fields

		private readonly MessageRegistry messageRegistry = new MessageRegistry();

		private readonly TypeRegistry typeRegistry = TypeRegistry.CreateDefault();

		#endregion

		[Test]
		public void SampleMessageFieldParams()
		{
			var defaultSerializer = new DefaultSerializer<SampleMessage>(this.messageRegistry, this.typeRegistry);
			var d = new MessageDispatcher<SampleApiClass>(this.messageRegistry, this.typeRegistry);
			using (var buf = new ThreadSafeWriteQueue(1024))
			{
				var orig = new SampleMessage { MessageId = defaultSerializer.MessageId, A = 10 };
				defaultSerializer.Serialize(buf, orig);
				var pos = buf.ReadMessage();
				Assert.IsTrue(d.Dispatch(buf, pos, defaultSerializer.MessageId, new SampleApiClass()));
			}
		}

	}
}