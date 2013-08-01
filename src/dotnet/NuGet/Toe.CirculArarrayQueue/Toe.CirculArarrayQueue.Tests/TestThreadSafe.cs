using System;
using System.Threading;

using NUnit.Framework;

namespace Toe.CircularArrayQueue.Tests
{
	[TestFixture]
	public class TestThreadSafe
	{
		#region Public Methods and Operators

		[Test]
		public void TestHuge()
		{
			using (var b = new ThreadSafeWriteQueue(1024))
			{
				var pos = b.Allocate(1024 - 2);
				b.Commit(pos);
				var a = b.ReadMessage();
				Assert.AreEqual(pos, a);
				Assert.AreEqual(-1, b.ReadMessage());

				pos = b.Allocate(1024 - 2);
				b.Commit(pos);
				a = b.ReadMessage();
				Assert.AreEqual(pos, a);
				Assert.AreEqual(-1, b.ReadMessage());

				Assert.Throws<OverflowException>(() => b.Allocate(1024 - 1));
			}
		}

		[Test]
		public void TestMultiThreading()
		{
			var threads = new Thread[4];
			int messageLength = 2;
			int maxNumMessages = 1024 * 1024 - 1;
			using (ThreadSafeWriteQueue b = new ThreadSafeWriteQueue(threads.Length * messageLength * maxNumMessages))
			{
				using (Semaphore semaphore = new Semaphore(0, threads.Length))
				{
					var messagesPerThread = maxNumMessages;
					for (int index = 0; index < threads.Length; index++)
					{
						var thread = threads[index] = new Thread(this.WriteProc);
						thread.Start(new Context { Buffer = b, Semaphore = semaphore, Count = messagesPerThread, Id = index });
					}
					semaphore.Release(threads.Length);
					var t1 = DateTime.Now;
					foreach (var thread in threads)
					{
						thread.Join();
					}
					var t2 = DateTime.Now;

					Console.WriteLine(
						"{0} sec ({1} per sec)",
						t2.Subtract(t1).TotalSeconds,
						messagesPerThread * threads.Length / t2.Subtract(t1).TotalSeconds);

					int count = 0;
					int[] countsPerThread = new int[threads.Length];

					int pos;
					while ((pos = b.ReadMessage()) >= 0)
					{
						++countsPerThread[b.ReadInt32(pos)];
						++count;
					}
					for (int index = 0; index < countsPerThread.Length; index++)
					{
						Console.WriteLine("Thread {0} sent {1} messages", index, countsPerThread[index]);
					}
					Assert.AreEqual(messagesPerThread * threads.Length, count);
				}
			}
		}

		[Test]
		public void TestOverflow()
		{
			using (var b = new ThreadSafeWriteQueue(1024))
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
		public void TestSafeRead()
		{
			using (var b = new ThreadSafeWriteQueue(8))
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

		#region Methods

		private void WriteProc(object obj)
		{
			Context context = (Context)obj;
			context.Semaphore.WaitOne();
			for (int i = 0; i < context.Count; ++i)
			{
				var pos = context.Buffer.Allocate(1);
				context.Buffer.WriteInt32(pos, context.Id);
				context.Buffer.Commit(pos);
			}
			Console.WriteLine("Thread {0} complete. {1} messages sent.", context.Id, context.Count);
		}

		#endregion

		public class Context
		{
			#region Public Properties

			public ThreadSafeWriteQueue Buffer { get; set; }

			public int Count { get; set; }

			public int Id { get; set; }

			public Semaphore Semaphore { get; set; }

			#endregion
		}
	}
}