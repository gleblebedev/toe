using Toe.CircularArrayQueue;

namespace Toe.Messaging
{
    internal class BinaryDeserializerContext
    {
        private IMessageQueue queue;
        private int pos;
        public BinaryDeserializerContext()
        {
        }

        public BinaryDeserializerContext Begin(IMessageQueue queue, int pos)
        {
            this.queue = queue;
            this.pos = pos;
            return this;
        }
    }
}