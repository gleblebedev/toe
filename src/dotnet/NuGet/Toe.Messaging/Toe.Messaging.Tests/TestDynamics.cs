using System;

using NUnit.Framework;

using Toe.CircularArrayQueue;
using Toe.Messaging.Types;

namespace Toe.Messaging.Tests
{
	[TestFixture]
	public class TypeRegistryTests
	{
		#region Public Methods and Operators

		[Test]
		public void Test1()
		{
			Console.WriteLine(string.Format("PropertyType.Int32 = 0x{0:X8} {0}", PropertyType.Int32));
			Console.WriteLine(string.Format("PropertyType.Single = 0x{0:X8} {0}", PropertyType.Single));
			Console.WriteLine(string.Format("PropertyType.String = 0x{0:X8} {0}", PropertyType.String));
			Assert.AreEqual(Hash.Eval("Int32"), PropertyType.Int32);
			Assert.AreEqual(Hash.Eval("Single"), PropertyType.Single);
			Assert.AreEqual(Hash.Eval("String"), PropertyType.String);
		}

		#endregion
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
				r.Serialize(buf, new { ser.MessageId, A = 1, B = 2 });
				dynamic a = new { };
				r.Deserialize(buf, buf.ReadMessage(), a);
			}
		}

		#endregion
	}
}