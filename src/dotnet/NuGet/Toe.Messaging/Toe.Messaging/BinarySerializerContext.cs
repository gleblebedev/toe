using Toe.CircularArrayQueue;

namespace Toe.Messaging
{
	internal class BinarySerializerContext
	{
		private IMessageQueue queue;

		private int pos;

		private int dymanicPos;

		public BinarySerializerContext()
		{
		}
		public BinarySerializerContext Allocate(IMessageQueue queue, int fixedLen, int dynamicLen)
		{
			this.queue = queue;
			this.pos = this.queue.Allocate(fixedLen + dynamicLen);
			this.dymanicPos = fixedLen + this.pos;
			return this;
		}
		public BinarySerializerContext Commit()
		{
			this.queue.Commit(this.pos);
			return this;
		}
	}
}