using System;
using System.IO;

using NUnit.Framework;

using Toe.Resources;
using Toe.Utils.Mesh.Marmalade.IwAnim;

namespace Toe.Utils.Mesh.Marmalade.Tests
{
	[TestFixture]
	public class TestSkel
	{
		#region Public Methods and Operators

		//[Test]
		//public void TestLegs()
		//{
		//    using (IResourceManager rm = new Toe.Resources.ResourceManager())
		//    {
		//        var r = new TextResourceReader(rm);
		//        var fileName = "male_skel_lod0.skel";
		//        using (var fileStream = File.OpenRead(fileName))
		//        {
		//            r.Load(fileStream, Path.GetDirectoryName(Path.GetFullPath(fileName)));
		//        }
		//    }
		//}

		//[Test]
		//public void TestMarmaladeFolder()
		//{
		//    using (IResourceManager rm = new Toe.Resources.ResourceManager())
		//    {
		//        var r = new TextResourceReader(rm);

		//        var s = new FolderTreeSearch(@"C:\Marmalade\6.2\examples\", "*.skel");
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