using System;

using NUnit.Framework;

namespace Toe.Utils.ToeMath.Tests
{
	[TestFixture]
	public class HalfTests
	{
		[Test]
		public void Dot()
		{
			Half4 a = new Half4(1, 2, 3, 4);
			Half4 b = new Half4(1, 2, 3, 4);
			half expected = (half)(1 + 4 + 9 + 16);
			Assert.AreEqual(expected, MathHelper.Dot(a, b));

		}

		[Test]
		public void Size()
		{

			Assert.AreEqual(2*4, Half4.SizeInBytes);
			Assert.AreEqual(2 * 3, Half3.SizeInBytes);

		}
	}
	[TestFixture]
	public class TestDouble
	{
		[Test]
		public void Dot()
		{
			Double4 a = new Double4(1, 2, 3, 4);
			Double4 b = new Double4(1, 2, 3, 4);
			Double expected = (Double)(1 + 4 + 9 + 16);
			Assert.AreEqual(expected, MathHelper.Dot(a, b));

		}
	}
}