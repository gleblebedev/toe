using System;
using System.Linq.Expressions;

using NUnit.Framework;

using Toe.CircularArrayQueue;

namespace Toe.Messaging.Tests
{
    public class BinarySerializer
    {
        private IMessageQueue queue;

        private int pos;

        private int dymanicPos;

        public BinarySerializer()
        {
        }
		public BinarySerializer Commit()
		{
			this.queue.Commit(pos);
			return this;
		}
		public BinarySerializer Allocate(IMessageQueue queue, int fixedLen, int dynamicLen)
        {
            this.queue = queue;
            pos = this.queue.Allocate(fixedLen + dynamicLen);
            this.dymanicPos = fixedLen + pos;
			return this;
        }

        public BinarySerializer WriteInt32(int i, int int32)
        {
            queue.WriteInt32(pos+i,int32);
            return this;
        }

		public BinarySerializer WriteString(int i, string text)
        {
            queue.WriteInt32(pos + i, dymanicPos);
            dymanicPos = queue.WriteString(dymanicPos, text);
			return this;
        }
    }
	[TestFixture]
	public class TestObjectMapping
	{
		#region Public Methods and Operators

		[Test]
		public void Message()
		{
			var reg = new MessageRegistry();
			var pos = reg.DefineMessage(Hash.Eval(typeof(Message).Name), 0);
			var omm = new ObjectMessageMap(reg);

			var r = omm.GetSerializer<SubMessage>();

			using (var buf = new ThreadSafeWriteQueue(1024))
			{
				var orig = new SubMessage { MessageId = int.MaxValue };
				r.Serialize(buf, orig);
				SubMessage message = new SubMessage();
				r.Deserialize(buf, buf.ReadMessage(), message);
				Assert.AreEqual(orig.MessageId, message.MessageId);
			}
		}
		BinarySerializer Allocate()
		{
			return new BinarySerializer();
		}
		void Release(BinarySerializer r)
		{
		}
	    [Test]
	    public void Experimental()
	    {
		    var serializer = new BinarySerializer();
	        Expression<Action<IMessageQueue, SubMessage>> serialize = (q, m) =>
					this.Release(Allocate().Allocate(q, 1 + 2, q.GetStringLength(m.Text)).WriteInt32(0, m.MessageId)
	                                                                         .WriteString(1, m.Text).Commit());
	    }

	    #endregion
	}
}