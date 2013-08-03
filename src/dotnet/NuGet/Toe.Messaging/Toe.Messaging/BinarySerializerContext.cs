using System;

using Toe.CircularArrayQueue;

namespace Toe.Messaging
{
	internal class BinarySerializerContext
	{
		#region Constants and Fields

		private int dymanicPos;

		private int pos;

		private IMessageQueue queue;

		#endregion

		#region Public Methods and Operators

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

		public BinarySerializerContext ReadInt32(int arg1, Func<int, int> arg2)
		{
			arg2(arg1);
			return this;
		}

		public BinarySerializerContext WriteFloat(int offset, float value)
		{
			this.queue.WriteFloat(offset + this.pos, value);
			return this;
		}

		public BinarySerializerContext WriteInt32(int offset, int value)
		{
			this.queue.WriteInt32(offset + this.pos, value);
			return this;
		}

		#endregion
	}
}