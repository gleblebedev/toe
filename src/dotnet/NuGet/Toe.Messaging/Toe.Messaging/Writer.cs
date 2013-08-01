using Toe.CircularArrayQueue;

namespace Toe.Messaging
{
	public delegate int Writer<T>(IMessageQueue queue, T message);
}