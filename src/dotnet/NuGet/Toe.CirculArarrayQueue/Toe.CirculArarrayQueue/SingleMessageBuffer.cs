using System;

namespace Toe.CircularArrayQueue
{
	/// <summary>
	/// Buffer for only one message at the time.
	/// </summary>
	public sealed class SingleMessageBuffer : IMessageQueue
	{
		#region Constants and Fields

		private readonly BufferItem[] messageBuffer;

		private readonly int size;

		private int messageSize;

		private int readPosition = -1;

		#endregion

		#region Constructors and Destructors

		public SingleMessageBuffer(int size)
		{
			this.size = size;
			this.messageBuffer = new BufferItem[this.size];
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
			if ((numDWords > this.size) || (this.messageSize != 0))
			{
				throw new OverflowException("message buffer overflow");
			}
			this.messageSize = numDWords;
			return 0;
		}

		/// <summary>
		/// Mark chunk as complete and ready to be readed.
		/// </summary>
		/// <param name="position">Position of the chunk, previously returned by Allocate.</param>
		public void Commit(int position)
		{
			this.readPosition = 0;
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
			return this.messageSize;
		}

		/// <summary>
		/// Read 4 byte floating point value at position.
		/// </summary>
		/// <param name="position">Absolute position to read.</param>
		/// <returns>Floating point value.</returns>
		public float ReadFloat(int position)
		{
			return this.messageBuffer[(position)].Single;
		}

		/// <summary>
		/// Read 4 byte integer value at position.
		/// </summary>
		/// <param name="position">Absolute position to read.</param>
		/// <returns>Integer value value.</returns>
		public int ReadInt32(int position)
		{
			return this.messageBuffer[(position)].Int32;
		}

		/// <summary>
		/// Read position of a next available chunk.
		/// </summary>
		/// <returns>Position at the queue buffer or -1 if there are no messages available.</returns>
		public int ReadMessage()
		{
			var r = this.readPosition;
			this.messageSize &= ~this.readPosition;
			this.readPosition = -1;
			return r;
		}

		/// <summary>
		/// Write 4 byte floating point value at absolute position.
		/// </summary>
		/// <param name="position">Absolute position to write at.</param>
		/// <param name="value">4 byte floating point value to write.</param>
		public void WriteFloat(int position, float value)
		{
			this.messageBuffer[(position)].Single = value;
		}

		/// <summary>
		/// Write 4 byte integer value at absolute position.
		/// </summary>
		/// <param name="position">Absolute position to write at.</param>
		/// <param name="value">4 byte integer value to write.</param>
		public void WriteInt32(int position, int value)
		{
			this.messageBuffer[(position)].Int32 = value;
		}

		#endregion
	}
}