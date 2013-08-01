using System;

namespace Toe.CircularArrayQueue
{
	/// <summary>
	/// Message queue interface.
	/// </summary>
	public interface IMessageQueue : IDisposable
	{
		#region Public Methods and Operators

		/// <summary>
		/// Allocate buffer range for message data.
		/// </summary>
		/// <param name="numDWords">Number of 4-byte double words in the message.</param>
		/// <returns>Position where range is allocated.</returns>
		int Allocate(int numDWords);

		/// <summary>
		/// Mark chunk as complete and ready to be readed.
		/// </summary>
		/// <param name="position">Position of the chunk, previously returned by Allocate.</param>
		void Commit(int position);

		/// <summary>
		/// Causes any buffered data to be written to the underlying device (network, etc).
		/// </summary>
		void Flush();

		/// <summary>
		/// Gets size of the chunk at position.
		/// </summary>
		/// <param name="position">Position of the chunk, previously returned by Allocate.</param>
		int GetSize(int position);

		/// <summary>
		/// Read 4 byte floating point value at position.
		/// </summary>
		/// <param name="position">Absolute position to read.</param>
		/// <returns>Floating point value.</returns>
		float ReadFloat(int position);

		/// <summary>
		/// Read 4 byte integer value at position.
		/// </summary>
		/// <param name="position">Absolute position to read.</param>
		/// <returns>Integer value value.</returns>
		int ReadInt32(int position);

		/// <summary>
		/// Read position of a next available chunk.
		/// </summary>
		/// <returns>Integer value value.</returns>
		int ReadMessage();

		/// <summary>
		/// Write 4 byte floating point value at absolute position.
		/// </summary>
		/// <param name="position">Absolute position to write at.</param>
		/// <param name="value">4 byte floating point value to write.</param>
		void WriteFloat(int position, float value);

		/// <summary>
		/// Write 4 byte integer value at absolute position.
		/// </summary>
		/// <param name="position">Absolute position to write at.</param>
		/// <param name="value">4 byte integer value to write.</param>
		void WriteInt32(int position, int value);

		#endregion
	}
}