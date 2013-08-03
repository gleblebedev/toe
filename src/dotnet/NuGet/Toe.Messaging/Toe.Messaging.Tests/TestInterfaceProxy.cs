using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using NUnit.Framework;

using Toe.CircularArrayQueue;

namespace Toe.Messaging.Tests
{
	public class BinarySerializer
	{
		#region Constants and Fields

		private int dymanicPos;

		private int pos;

		private IMessageQueue queue;

		#endregion

		#region Public Methods and Operators

		public BinarySerializer Allocate(IMessageQueue queue, int fixedLen, int dynamicLen)
		{
			this.queue = queue;
			this.pos = this.queue.Allocate(fixedLen + dynamicLen);
			this.dymanicPos = fixedLen + this.pos;
			return this;
		}

		public BinarySerializer Commit()
		{
			this.queue.Commit(this.pos);
			return this;
		}

		public BinarySerializer ReadAt(int i)
		{
			this.pos = i;
			return this;
		}

		public BinarySerializer ReadInt32(int offset, Func<int, int> func)
		{
			func(this.queue.ReadInt32(this.pos + offset));
			return this;
		}

		public BinarySerializer WriteInt32(int i, int int32)
		{
			this.queue.WriteInt32(this.pos + i, int32);
			return this;
		}

		public BinarySerializer WriteString(int i, string text)
		{
			this.queue.WriteInt32(this.pos + i, this.dymanicPos);
			this.dymanicPos = this.queue.WriteString(this.dymanicPos, text);
			return this;
		}

		#endregion
	}

	[TestFixture]
	public class TestObjectMapping
	{
		#region Public Methods and Operators

		[Test]
		public void Message()
		{
			var reg = new MessageRegistry();
			var pos = reg.DefineMessage(Hash.Eval(typeof(SubMessage).Name), 0);
			var omm = new ObjectMessageMap(reg);

			var r = omm.GetSerializer<SubMessage>();

			using (var buf = new ThreadSafeWriteQueue(1024))
			{
				var orig = new SubMessage
					{
						MessageId = int.MaxValue,
						A = 1,
						B = 2,
						C = 3,
						Text = "aaa‚Ô‡‡‚",
					};
				var orig2 = new SubMessage
				{
					MessageId = int.MaxValue,
					A = 4,
					B = 5,
					C = 6,
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

		#region Methods

		private BinarySerializer Allocate()
		{
			return new BinarySerializer();
		}

		private void Release(BinarySerializer r)
		{
		}

		#endregion
	}
}