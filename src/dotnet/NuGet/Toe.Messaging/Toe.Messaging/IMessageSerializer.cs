using Toe.CircularArrayQueue;

namespace Toe.Messaging
{
	public interface IMessageSerializer
	{
		int Serialize(IMessageQueue queue, object value);
		void Deserialize(IMessageQueue queue, int pos, object value);
	}

	public interface IMessageSerializer<in T> : IMessageSerializer
	{
		int Serialize(IMessageQueue queue, T value);
		void Deserialize(IMessageQueue queue, int pos, T value);
	}
}