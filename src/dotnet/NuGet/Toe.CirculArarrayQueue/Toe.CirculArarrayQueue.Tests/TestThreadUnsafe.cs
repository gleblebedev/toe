using System;

using NUnit.Framework;

namespace Toe.CircularArrayQueue.Tests
{
	[TestFixture]
	public class TestThreadUnsafe
	{
		#region Public Methods and Operators

		[Test]
		public void TestOverflow()
		{
			using (var b = new ThreadUnsafeQueue(1024))
			{
				int i = 1024 / (2);
				while (i > 1)
				{
					b.Commit(b.Allocate(1));
					--i;
				}
				Assert.Throws<OverflowException>(() => b.Allocate(1));
			}
		}

		[Test]
		public void TestPerformance()
		{
			var numMessages = 4 * 1024 * 1024 - 1;
			using (var b = new ThreadUnsafeQueue(2 * numMessages))
			{
				var t1 = DateTime.Now;
				for (int i = 0; i < numMessages; i++)
				{
					b.Commit(b.Allocate(1));
				}
				var t2 = DateTime.Now;

				Console.WriteLine(
					"Thread unsafe: {2} messages {0} sec ({1} per sec)",
					t2.Subtract(t1).TotalSeconds,
					numMessages / t2.Subtract(t1).TotalSeconds,
					numMessages);

				int count = 0;
				int pos;
				while ((pos = b.ReadMessage()) >= 0)
				{
					++count;
				}
				Assert.AreEqual(numMessages, count);
			}
		}

		[Test]
		public void TestSafeRead()
		{
			using (var b = new ThreadUnsafeQueue(8))
			{
				b.Commit(b.Allocate(2));
				b.Commit(b.Allocate(2));
				b.ReadMessage();
				Assert.Throws<OverflowException>(() => b.Allocate(2));
				b.ReadMessage();
				b.Commit(b.Allocate(2));
			}
		}

		#endregion
	}
}