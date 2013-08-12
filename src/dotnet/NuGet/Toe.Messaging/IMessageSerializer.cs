using Toe.CircularArrayQueue;

namespace Toe.Messaging
{
	public interface IMessageSerializer
	{
		#region Public Methods and Operators

		void Deserialize(IMessageQueue queue, int pos, object value);

		void Serialize(IMessageQueue queue, object value);

		#endregion
	}

	public interface IMessageSerializer<in T> : IMessageSerializer
	{
		#region Public Methods and Operators

		void Deserialize(IMessageQueue queue, int pos, T value);

		void Serialize(IMessageQueue queue, T value);

		#endregion
	}
}