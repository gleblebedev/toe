using System;
using System.IO;

using NUnit.Framework;

using Toe.Utils.Mesh.Marmalade.IwGraphics;

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

		[Test]
		public void TestMarmaladeFolder()
		{
			var r = new TextResourceReader();

			var s = new FolderTreeSearch(@"C:\Marmalade\6.2\examples\", "*.geo");
			foreach (var file in s)
			{
				Console.WriteLine(file);
				using (var fileStream = File.OpenRead(file))
				{
					r.Load(fileStream, Path.GetDirectoryName(Path.GetFullPath(file)));
				}
			}
		}

		#endregion
	}
}