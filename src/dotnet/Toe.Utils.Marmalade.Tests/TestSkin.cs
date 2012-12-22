using System;
using System.IO;

using NUnit.Framework;

using Toe.Utils.Mesh.Marmalade.IwAnim;

namespace Toe.Utils.Mesh.Marmalade.Tests
{
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

		[Test]
		public void TestMarmaladeFolder()
		{
			var r = new SkinReader();

			var s = new FolderTreeSearch(@"C:\Marmalade\6.2\examples\", "*.skin");
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