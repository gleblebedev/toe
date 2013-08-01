using System;
using System.Threading;

namespace Toe.CircularArrayQueue
{
	/// <summary>
	/// Message queue implementation with thread safe allocations.
	/// </summary>
	[CLSCompliant(true)]
	public sealed class ThreadSafeWriteQueue : IMessageQueue
	{
		#region Constants and Fields

		private readonly int mask;

		private readonly BufferItem[] messageBuffer;

		private readonly int size;

		private int nextReadPosition;

		private int readPosition;

		private int writePosition;

		#endregion

		#region Constructors and Destructors

		public ThreadSafeWriteQueue(int recommendedSize)
		{
			this.mask = recommendedSize - 1;
			this.mask |= this.mask >> 1;
			this.mask |= this.mask >> 2;
			this.mask |= this.mask >> 4;
			this.mask |= this.mask >> 8;
			this.mask |= this.mask >> 16;
			this.size = this.mask + 1;

			this.messageBuffer = new BufferItem[this.size];

			this.writePosition = 0;
			this.nextReadPosition = this.readPosition = 0;
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Allocate buffer range for message data.
		/// </summary>
		/// <param name="numDWords">Number of 4-byte double words in the message.</param>
		/// <returns>Position where range is allocated.</returns>
		public int Allocate(int numDWords)
		{
			retry:
			int position = this.writePosition;
			var newPosition = (position + numDWords + 1) & this.mask;
			var oldData = this.messageBuffer[position].Int32;
			if (position != this.writePosition)
			{
				goto retry;
			}

			if (numDWords > ((newPosition - this.readPosition) & this.mask))
			{
				throw new OverflowException("message buffer overflow");
			}

			if (oldData != Interlocked.CompareExchange(ref this.messageBuffer[position].Int32, ~numDWords, oldData))
			{
				goto retry;
			}
			if (position != Interlocked.CompareExchange(ref this.writePosition, newPosition, position))
			{
				goto retry;
			}
			//Trace.WriteLine(string.Format("Allocated at {0}", position));
			return (position + 1);
		}

		/// <summary>
		/// Mark chunk as complete and ready to be readed.
		/// </summary>
		/// <param name="position">Position of the chunk, previously returned by Allocate.</param>
		public void Commit(int position)
		{
			var index0 = ((position - 1) & this.mask);
			this.messageBuffer[index0].Int32 = ~this.messageBuffer[index0].Int32;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
		}

		/// <summary>
		/// Causes any buffered data to be written to the underlying device (network, etc).
		/// </summary>
		public void Flush()
		{
		}

		/// <summary>
		/// Gets size of the chunk at position.
		/// </summary>
		/// <param name="position">Position of the chunk, previously returned by Allocate.</param>
		public int GetSize(int position)
		{
			var size = this.messageBuffer[((position - 1) & this.mask)].Int32;
			return size < 0 ? ~size : size;
		}

		/// <summary>
		/// Read 4 byte floating point value at position.
		/// </summary>
		/// <param name="position">Absolute position to read.</param>
		/// <returns>Floating point value.</returns>
		public float ReadFloat(int position)
		{
			return this.messageBuffer[(position & this.mask)].Single;
		}

		/// <summary>
		/// Read 4 byte integer value at position.
		/// </summary>
		/// <param name="position">Absolute position to read.</param>
		/// <returns>Integer value value.</returns>
		public int ReadInt32(int position)
		{
			return this.messageBuffer[(position & this.mask)].Int32;
		}

		/// <summary>
		/// Read position of a next available chunk.
		/// </summary>
		/// <returns>Integer value value.</returns>
		public int ReadMessage()
		{
			var position = this.readPosition = this.nextReadPosition;
			if (position == this.writePosition || this.ReadInt32(position) < 0)
			{
				return -1;
			}
			this.nextReadPosition = (position + this.ReadInt32(position) + 1) & this.mask;
			return position + 1;

			////"Thread safe" reading draft
			//retry:
			//var position = this.readPosition;
			//var len = this.messageBuffer[position].Int;
			//if (position == this.writePosition || len < 0)
			//{
			//	return -1;
			//}
			//var newPosition = (position + len + 1) & this.mask;
			//if (position != Interlocked.CompareExchange(ref this.readPosition, newPosition, position))
			//{
			//	goto retry;
			//}
			//return position + 1;
		}

		/// <summary>
		/// Write 4 byte floating point value at absolute position.
		/// </summary>
		/// <param name="position">Absolute position to write at.</param>
		/// <param name="value">4 byte floating point value to write.</param>
		public void WriteFloat(int position, float value)
		{
			this.messageBuffer[(position & this.mask)].Single = value;
		}

		/// <summary>
		/// Write 4 byte integer value at absolute position.
		/// </summary>
		/// <param name="position">Absolute position to write at.</param>
		/// <param name="value">4 byte integer value to write.</param>
		public void WriteInt32(int position, int value)
		{
			this.messageBuffer[(position & this.mask)].Int32 = value;
		}

		#endregion
	}
}