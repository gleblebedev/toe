using System.IO;

using NUnit.Framework;

namespace Toe.Utils.Mesh.Marmalade.Tests
{
	[TestFixture]
	public class TestGeo
	{
		#region Public Methods and Operators

		[Test]
		public void TestLegs()
		{
			var r = new GeoReader();
			using (var fileStream = File.OpenRead("male_legs_trousers0_lod0.geo"))
			{
				r.Load(fileStream);
			}
		}

		#endregion
	}

	[TestFixture]
	public class TestSkel
	{
		#region Public Methods and Operators

		[Test]
		public void TestLegs()
		{
			var r = new SkelReader();
			using (var fileStream = File.OpenRead("male_skel_lod0.skel"))
			{
				r.Load(fileStream);
			}
		}

		#endregion
	}

	[TestFixture]
	public class TestSkin
	{
		#region Public Methods and Operators

		[Test]
		public void TestLegs()
		{
			var r = new SkinReader();
			using (var fileStream = File.OpenRead("male_legs_trousers0_lod0.skin"))
			{
				r.Load(fileStream);
			}
		}

		#endregion
	}
}