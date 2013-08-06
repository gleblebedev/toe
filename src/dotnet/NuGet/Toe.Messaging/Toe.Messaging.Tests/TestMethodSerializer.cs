using NUnit.Framework;

using Toe.CircularArrayQueue;
using Toe.Messaging.Types;

namespace Toe.Messaging.Tests
{
	[TestFixture]
	public class TestMessageDispatcherClassParam
	{
		#region Constants and Fields

		private readonly MessageRegistry messageRegistry = new MessageRegistry();

		private readonly TypeRegistry typeRegistry = TypeRegistry.CreateDefault();

		#endregion

		#region Public Methods and Operators

		[Test]
		public void SampleMessageClassParam()
		{
			var defaultSerializer = new DefaultSerializer<SampleMessage>(this.messageRegistry, this.typeRegistry);
			var d = new MessageDispatcher<SampleApiClass>(this.messageRegistry, this.typeRegistry);
			using (var buf = new ThreadSafeWriteQueue(1024))
			{
				var orig = new SampleMessage { MessageId = defaultSerializer.MessageId, };
				defaultSerializer.Serialize(buf, orig);
				var pos = buf.ReadMessage();
				d.Dispatch(buf, pos, defaultSerializer.MessageId, new SampleApiClass());
				Assert.Fail();
			}
		}

		#endregion

		public class SampleApiClass
		{
			#region Public Methods and Operators

			public bool OnSampleMessage(SampleMessage message)
			{
				Assert.Pass("Ok!");
				return true;
			}

			#endregion
		}

		public class SampleMessage : Message
		{
		}
	}
}