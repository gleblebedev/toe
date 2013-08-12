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
			Console.WriteLine(string.Format("PropertyType.Int32 = 0x{0:X8} {0}", PropertyTypes.Int32));
			Console.WriteLine(string.Format("PropertyType.Single = 0x{0:X8} {0}", PropertyTypes.Single));
			Console.WriteLine(string.Format("PropertyType.String = 0x{0:X8} {0}", PropertyTypes.String));
			Assert.AreEqual(Hash.Eval("Int32"), PropertyTypes.Int32);
			Assert.AreEqual(Hash.Eval("Single"), PropertyTypes.Single);
			Assert.AreEqual(Hash.Eval("String"), PropertyTypes.String);
			Assert.AreEqual(Hash.Eval("QuaternionXYZW"), PropertyTypes.QuaternionXYZW);
			Assert.AreEqual(Hash.Eval("VectorXYZW"), PropertyTypes.VectorXYZW);
			Assert.AreEqual(Hash.Eval("VectorXYZ"), PropertyTypes.VectorXYZ);
			Assert.AreEqual(Hash.Eval("VectorXY"), PropertyTypes.VectorXY);
		}

		#endregion
	}
}