using NUnit.Framework;

using Toe.CircularArrayQueue;
using Toe.Messaging.Types;

namespace Toe.Messaging.Tests
{
	[TestFixture]
	public class TestDefaultSerializer
	{
		#region Constants and Fields

		private readonly MessageRegistry messageRegistry = new MessageRegistry();
		private readonly TypeRegistry typeRegistry = TypeRegistry.CreateDefault();

		#endregion

		#region Public Methods and Operators

		[Test]
		public void EmptyString()
		{
			var r = new DefaultSerializer<SubMessage>(this.messageRegistry, typeRegistry);

			using (var buf = new ThreadSafeWriteQueue(1024))
			{
				var orig = new SubMessage { Text = string.Empty, };
				r.Serialize(buf, orig);

				SubMessage message = new SubMessage();

				r.Deserialize(buf, buf.ReadMessage(), message);
				Assert.AreEqual(orig.MessageId, message.MessageId);
				Assert.IsTrue(string.IsNullOrEmpty(message.Text));
			}
		}

		[Test]
		public void FourBytesString()
		{
			var r = new DefaultSerializer<SubMessage>(this.messageRegistry, typeRegistry);

			using (var buf = new ThreadSafeWriteQueue(1024))
			{
				var orig = new SubMessage { Text = "aaaa", };
				r.Serialize(buf, orig);
				r.Serialize(buf, orig);

				SubMessage message = new SubMessage();

				r.Deserialize(buf, buf.ReadMessage(), message);
				Assert.AreEqual(orig.MessageId, message.MessageId);
				Assert.AreEqual(orig.Text, message.Text);

				r.Deserialize(buf, buf.ReadMessage(), message);
				Assert.AreEqual(orig.MessageId, message.MessageId);
				Assert.AreEqual(orig.Text, message.Text);
			}
		}

		[Test]
		public void NullString()
		{
			var r = new DefaultSerializer<SubMessage>(this.messageRegistry, typeRegistry);

			using (var buf = new ThreadSafeWriteQueue(1024))
			{
				var orig = new SubMessage { Text = null, };
				r.Serialize(buf, orig);

				SubMessage message = new SubMessage();

				r.Deserialize(buf, buf.ReadMessage(), message);
				Assert.AreEqual(orig.MessageId, message.MessageId);
				Assert.IsTrue(string.IsNullOrEmpty(message.Text));
			}
		}

		[Test]
		public void ThreeBytesString()
		{
			var r = new DefaultSerializer<SubMessage>(this.messageRegistry, typeRegistry);

			using (var buf = new ThreadSafeWriteQueue(1024))
			{
				var orig = new SubMessage { Text = "aaa", };
				r.Serialize(buf, orig);
				r.Serialize(buf, orig);

				SubMessage message = new SubMessage();

				r.Deserialize(buf, buf.ReadMessage(), message);
				Assert.AreEqual(orig.MessageId, message.MessageId);
				Assert.AreEqual(orig.Text, message.Text);

				r.Deserialize(buf, buf.ReadMessage(), message);
				Assert.AreEqual(orig.MessageId, message.MessageId);
				Assert.AreEqual(orig.Text, message.Text);
			}
		}

		[Test]
		public void TwoMessages()
		{
			var r = new DefaultSerializer<SubMessage>(this.messageRegistry, typeRegistry);

			using (var buf = new ThreadSafeWriteQueue(1024))
			{
				var orig = new SubMessage { MessageId = int.MaxValue, A = 1, B = 2, C = 3, Text = "aaa‚Ô‡‡‚", };
				var orig2 = new SubMessage { MessageId = int.MaxValue, A = 4, B = 5, C = 6, Text = "‚Ô‡‡‚zz", };
				r.Serialize(buf, orig);
				r.Serialize(buf, orig2);

				SubMessage message = new SubMessage();

				r.Deserialize(buf, buf.ReadMessage(), message);
				Assert.AreEqual(orig.MessageId, message.MessageId);
				Assert.AreEqual(orig.A, message.A);
				Assert.AreEqual(orig.B, message.B);
				Assert.AreEqual(orig.C, message.C);
				Assert.AreEqual(orig.Text, message.Text);

				r.Deserialize(buf, buf.ReadMessage(), message);
				Assert.AreEqual(orig2.MessageId, message.MessageId);
				Assert.AreEqual(orig2.A, message.A);
				Assert.AreEqual(orig2.B, message.B);
				Assert.AreEqual(orig2.C, message.C);
				Assert.AreEqual(orig2.Text, message.Text);
			}
		}

		#endregion
	}
}