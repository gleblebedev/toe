using Toe.CircularArrayQueue;

namespace Toe.Messaging
{
	internal class BinaryDeserializerContext
	{
		#region Constants and Fields

		private int pos;

		private IMessageQueue queue;

		#endregion

		#region Public Methods and Operators

		public BinaryDeserializerContext Begin(IMessageQueue queue, int pos)
		{
			this.queue = queue;
			this.pos = pos;
			return this;
		}

		#endregion
	}
}