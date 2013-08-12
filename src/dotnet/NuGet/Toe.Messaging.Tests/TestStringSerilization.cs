using System.Text;

using NUnit.Framework;

using Toe.CircularArrayQueue;

namespace Toe.Messaging.Tests
{
	[TestFixture]
	public class TestStringSerialization
	{
		#region Public Methods and Operators

		[Test]
		public void Test()
		{
			for (int i = char.MinValue; i <= char.MaxValue; ++i)
			{
				if (i != '\0')
				{
					var str = ((char)i).ToString();
					Assert.AreEqual(Encoding.UTF8.GetByteCount(str), ExtensionMethods.GetByteCount(str));
				}
			}
		}

		#endregion
	}
}