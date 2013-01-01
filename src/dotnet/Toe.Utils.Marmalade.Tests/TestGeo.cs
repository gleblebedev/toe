using System;
using System.IO;
using System.Resources;

using NUnit.Framework;

using Toe.Resources;

namespace Toe.Utils.Mesh.Marmalade.Tests
{
	[TestFixture]
	public class TestGeo
	{
		#region Public Methods and Operators

		//[Test]
		//public void TestLegs()
		//{
		//    using (IResourceManager rm = new Toe.Resources.ResourceManager())
		//    {
		//        var r = new GeoReader(rm);
		//        using (var fileStream = File.OpenRead("male_legs_trousers0_lod0.geo"))
		//        {
		//            r.Load(fileStream);
		//        }
		//    }
		//}

		//[Test]
		//public void TestMarmaladeFolder()
		//{
		//    using (IResourceManager rm = new Toe.Resources.ResourceManager())
		//    {
		//        var r = new TextResourceReader(rm);

		//        var s = new FolderTreeSearch(@"C:\Marmalade\6.2\examples\", "*.geo");
		//        foreach (var file in s)
		//        {
		//            Console.WriteLine(file);
		//            using (var fileStream = File.OpenRead(file))
		//            {
		//                r.Load(fileStream, Path.GetDirectoryName(Path.GetFullPath(file)));
		//            }
		//        }
		//    }
		//}

		#endregion
	}
}