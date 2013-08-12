using System;

using NUnit.Framework;

namespace Toe.CircularArrayQueue.Tests
{
	[TestFixture]
	public class TestSignleBuffer
	{
		[Test]
		public void SizeTest()
		{
			int totalSize = 16;
			using (var b = new SingleMessageBuffer(totalSize))
			{
				b.Allocate(totalSize);
			}
		}

		[Test]
		public void ComplexTest()
		{
			int totalSize = 16;
			int testSize = 3;
			using (var b = new SingleMessageBuffer(totalSize))
			{
				Assert.Throws<OverflowException>(() => b.Allocate(totalSize+1));
				var pos = b.Allocate(testSize);
				Assert.AreEqual(testSize, b.GetSize(pos));
				Assert.Throws<OverflowException>(() => b.Allocate(1));
				b.Commit(pos);
				Assert.Throws<OverflowException>(() => b.Allocate(1));
				var r = b.ReadMessage();
				Assert.AreEqual(testSize, b.GetSize(r));
				
				Assert.AreEqual(pos,r);
				Assert.Throws<OverflowException>(() => b.Allocate(1));
				Assert.AreEqual(-1, b.ReadMessage());
				b.Allocate(totalSize);
			}
		}
	}
}