using System;
using System.Linq.Expressions;

using NUnit.Framework;

using Toe.CircularArrayQueue;

namespace Toe.Messaging.Tests
{
    public class BinarySerializer
    {
        private readonly IMessageQueue queue;

        private readonly int pos;

        private int dymanicPos;

        public BinarySerializer(IMessageQueue queue,int fixedLen, int dynamicLen)
        {
            this.queue = queue;
            pos = this.queue.Allocate(fixedLen + dynamicLen);
            this.dymanicPos = fixedLen + pos;
        }

        public BinarySerializer WriteInt32(int i, int int32)
        {
            queue.WriteInt32(pos+i,int32);
            return this;
        }

        public void WriteString(int i, string text)
        {
            queue.WriteInt32(pos + i, dymanicPos);
            dymanicPos = queue.WriteString(dymanicPos, text);
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

			var r = omm.GetSerializer<Message>();

			using (var buf = new ThreadSafeWriteQueue(1024))
			{
				var orig = new Message { MessageId = int.MaxValue };
				r.Serialize(buf, orig);
				Message message = new Message();
				r.Deserialize(buf, buf.ReadMessage(), message);
				Assert.AreEqual(orig.MessageId, message.MessageId);
			}
		}

	    [Test]
	    public void Experimental()
	    {
	        Expression<Action<IMessageQueue, SubMessage>> serialize = (q, m) =>
	                new BinarySerializer(q, 1 + 2, q.GetStringLength(m.Text)).WriteInt32(0, m.MessageId)
	                                                                         .WriteString(1, m.Text);
	    }

	    #endregion
	}
}