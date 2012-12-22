using System;
using System.IO;

using NUnit.Framework;

using Toe.Utils.Mesh.Marmalade.IwAnim;

namespace Toe.Utils.Mesh.Marmalade.Tests
{
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

		[Test]
		public void TestMarmaladeFolder()
		{
			var r = new SkelReader();

			var s = new FolderTreeSearch(@"C:\Marmalade\6.2\examples\", "*.skel");
			foreach (var file in s)
			{
				Console.WriteLine(file);
				using (var fileStream = File.OpenRead(file))
				{
					r.Load(fileStream);
				}
			}
		}


		#endregion
	}
}