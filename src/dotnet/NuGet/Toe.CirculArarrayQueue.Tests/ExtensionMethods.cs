using System.Text;

using NUnit.Framework;

namespace Toe.CircularArrayQueue.Tests
{
	[TestFixture]
	public class ExtensionMethods
	{
		#region Public Methods and Operators

		[Test]
		public void TestFloat()
		{
			using (var b = new ThreadSafeWriteQueue(10))
			{
				var pos = b.Allocate(9);

				this.TestFloat(b, pos, float.MinValue);
				this.TestFloat(b, pos, float.MaxValue);
				this.TestFloat(b, pos, float.Epsilon);
				this.TestFloat(b, pos, 0);
				this.TestFloat(b, pos, 1.1f);
			}
		}

		[Test]
		public void TestInt64()
		{
			using (var b = new ThreadSafeWriteQueue(10))
			{
				var pos = b.Allocate(9);

				this.TestInt64(b, pos, long.MinValue);
				this.TestInt64(b, pos, long.MaxValue);
				this.TestInt64(b, pos, int.MaxValue);
				this.TestInt64(b, pos, int.MinValue);
				this.TestInt64(b, pos, 0);
			}
		}

		[Test]
		public void TestString()
		{
			using (var b = new ThreadUnsafeQueue(1024))
			{
				for (int i = 1; i < char.MaxValue; i++)
				{
					var c = ((char)i).ToString();
					var testTest = c + c + c;
					var bytes = Encoding.UTF8.GetBytes(testTest);
					var len = b.GetStringLength(testTest);
					Assert.AreEqual(len, (bytes.Length + 4) >> 2, string.Format("Wrong size of char {0} ({1})", i, c));
				}
			}
		}

		#endregion

		#region Methods

		private void TestFloat(IMessageQueue q, int pos, float value)
		{
			q.WriteFloat(pos, value);
			Assert.AreEqual(value, q.ReadFloat(pos));
		}

		private void TestInt64(IMessageQueue q, int pos, long value)
		{
			q.WriteInt64(pos, value);
			Assert.AreEqual(value, q.ReadInt64(pos));
		}

		#endregion
	}
}