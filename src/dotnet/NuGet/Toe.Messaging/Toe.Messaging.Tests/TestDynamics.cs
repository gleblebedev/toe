using NUnit.Framework;

using Toe.CircularArrayQueue;

namespace Toe.Messaging.Tests
{
	[TestFixture]
	public class TestDynamics
	{
		#region Public Methods and Operators

		[Test]
		public void TestDynamicObject()
		{
			var reg = new MessageRegistry();
			var pos = reg.DefineMessage(1, 0);
			var omm = new ObjectMessageMap(reg);

			var r = omm.GetSerializer<dynamic>();

			using (var buf = new ThreadSafeWriteQueue(1024))
			{
				r.Serialize(buf, new { MessageId = 1, A = 1, B = 2 });
				dynamic a = new { };
				r.Deserialize(buf, buf.ReadMessage(), a);
			}
		}

		#endregion
	}
}