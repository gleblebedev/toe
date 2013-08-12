using NUnit.Framework;

using Toe.CircularArrayQueue;
using Toe.Messaging.Types;

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
			var typeRegistry = TypeRegistry.CreateDefault();
			var ser = new DefaultSerializer<SubMessage>(reg, typeRegistry);
			var r = new DynamicSerializer(reg, typeRegistry);

			using (var buf = new ThreadSafeWriteQueue(1024))
			{
				dynamic src = new SubMessage { MessageId = ser.MessageId, A = 1, B = 2 };
				r.Serialize(buf, src);
				dynamic a = new { };
				r.Deserialize(buf, buf.ReadMessage(), a);
			}
		}

		#endregion
	}
}