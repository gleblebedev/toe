using NUnit.Framework;

using Toe.CircularArrayQueue;

namespace Toe.Messaging.Tests
{
	[TestFixture]
	public class TestObjectMapping
	{
		#region Public Methods and Operators

		[Test]
		public void Message()
		{
			var reg = new MessageRegistry();
			var pos = reg.DefineMessage(Hash.Eval(typeof(Message).Name), 0);
			var omm = new ObjectMessageMap(reg);

			var r = omm.GetSerializer<Message>();

			using (var buf = new ThreadSafeWriteQueue(1024))
			{
				var orig = new Message { M = int.MaxValue };
				r.Serialize(buf, orig);
				Message message = new Message();
				r.Deserialize(buf, buf.ReadMessage(), message);
				Assert.AreEqual(orig.M, message.M);
			}
		}

		#endregion
	}
}