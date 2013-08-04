using System;

using NUnit.Framework;

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
			Assert.AreEqual(Hash.Eval("VectorXYZ"), PropertyType.VectorXYZ);
		}

		#endregion
	}
}