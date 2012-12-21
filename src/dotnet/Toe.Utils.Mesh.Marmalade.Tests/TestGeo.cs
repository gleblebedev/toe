using System.IO;

using NUnit.Framework;

namespace Toe.Utils.Mesh.Marmalade.Tests
{
	[TestFixture]
	public class TestGeo
	{
		[Test]
		public void TestLegs()
		{
			var r = new GeoReader();
			using (var fileStream = File.OpenRead("male_legs_trousers0_lod0.geo"))
			{
				r.Load(fileStream);
			}
		}
	}
}