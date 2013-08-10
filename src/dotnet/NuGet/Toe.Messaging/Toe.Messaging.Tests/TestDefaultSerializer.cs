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
			var r = new DefaultSerializer<SubMessage>(this.messageRegistry, this.typeRegistry);

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
		public void TestVec3()
		{
			var r = new DefaultSerializer<SingleMessage<Vector3>>(this.messageRegistry, this.typeRegistry,3);
			using (var buf = new ThreadSafeWriteQueue(1024))
			{
				var orig = new SingleMessage<Vector3> { MessageId = r.MessageId, A = new Vector3(1.1f, 2.2f, 3.3f) };
				r.Serialize(buf, orig);

				SingleMessage<Vector3> message = new SingleMessage<Vector3>();

				r.Deserialize(buf, buf.ReadMessage(), message);
				Assert.AreEqual(orig.MessageId, message.MessageId);
				Assert.AreEqual(orig.A, message.A);
			}
			
		}
		[Test]
		public void TestVec2()
		{
			var r = new DefaultSerializer<SingleMessage<Vector2>>(this.messageRegistry, this.typeRegistry,2);
			using (var buf = new ThreadSafeWriteQueue(1024))
			{
				var orig = new SingleMessage<Vector2> { MessageId = r.MessageId, A = new Vector2(1.1f, 2.2f) };
				r.Serialize(buf, orig);

				SingleMessage<Vector2> message = new SingleMessage<Vector2>();

				r.Deserialize(buf, buf.ReadMessage(), message);
				Assert.AreEqual(orig.MessageId, message.MessageId);
				Assert.AreEqual(orig.A, message.A);
			}

		}
		[Test]
		public void TestVec4()
		{
			var r = new DefaultSerializer<SingleMessage<Vector4>>(this.messageRegistry, this.typeRegistry,4);
			using (var buf = new ThreadSafeWriteQueue(1024))
			{
				var orig = new SingleMessage<Vector4> { MessageId = r.MessageId, A = new Vector4(1.1f, 2.2f, 3.3f,( 4.4f)) };
				r.Serialize(buf, orig);

				SingleMessage<Vector4> message = new SingleMessage<Vector4>();

				r.Deserialize(buf, buf.ReadMessage(), message);
				Assert.AreEqual(orig.MessageId, message.MessageId);
				Assert.AreEqual(orig.A, message.A);
			}

		}
		[Test]
		public void TestQuaternion()
		{
			var r = new DefaultSerializer<SingleMessage<Quaternion>>(this.messageRegistry, this.typeRegistry);
			using (var buf = new ThreadSafeWriteQueue(1024))
			{
				var orig = new SingleMessage<Quaternion> { MessageId = r.MessageId, A = new Quaternion(1.1f, 2.2f, 3.3f, (4.4f)) };
				r.Serialize(buf, orig);

				SingleMessage<Quaternion> message = new SingleMessage<Quaternion>();

				r.Deserialize(buf, buf.ReadMessage(), message);
				Assert.AreEqual(orig.MessageId, message.MessageId);
				Assert.AreEqual(orig.A, message.A);
			}

		}
		[Test]
		public void FourBytesString()
		{
			var r = new DefaultSerializer<SubMessage>(this.messageRegistry, this.typeRegistry);

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
			var r = new DefaultSerializer<SubMessage>(this.messageRegistry, this.typeRegistry);

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
			var r = new DefaultSerializer<SubMessage>(this.messageRegistry, this.typeRegistry);

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
			var r = new DefaultSerializer<SubMessage>(this.messageRegistry, this.typeRegistry);

			using (var buf = new ThreadSafeWriteQueue(1024))
			{
				var orig = new SubMessage
					{ MessageId = int.MaxValue, A = int.MinValue, B = float.Epsilon, C = uint.MinValue, Byte = 1, Text = "aaa‚Ô‡‡‚", };
				var orig2 = new SubMessage
					{
						MessageId = int.MaxValue,
						A = int.MaxValue,
						B = float.MaxValue,
						C = uint.MaxValue,
						Byte = 255,
						Text = "‚Ô‡‡‚zz",
					};
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