using System.Text;

using NUnit.Framework;

using Toe.CircularArrayQueue;
using Toe.Messaging.Types;

namespace Toe.Messaging.Tests
{
	[TestFixture]
	public class TestStringSerilization
	{
		[Test]
		public void Test()
		{
			for (int i=char.MinValue; i<=char.MaxValue;++i)
			{
				if (i != '\0')
				{
					var str = ((char)i).ToString();
					Assert.AreEqual(Encoding.UTF8.GetByteCount(str), CircularArrayQueue.ExtensionMethods.GetByteCount(str));
				}
			}
		}
	}
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