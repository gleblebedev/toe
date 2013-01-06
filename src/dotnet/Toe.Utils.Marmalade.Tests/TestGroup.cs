using System;

using Autofac;

using NUnit.Framework;

using Toe.Resources;

namespace Toe.Utils.Mesh.Marmalade.Tests
{
	[TestFixture]
	public class TestGroup : BaseTest
	{
		#region Public Methods and Operators

		[Test]
		public void TestGroupBin()
		{
			using (var rm = this.Container.Resolve<IResourceManager>())
			{
				var file = @"C:\GitHub\toe\src\marmalade\data-ram\data-gles1\male_lod0.group.bin";
				Console.WriteLine(file);
				var f = rm.EnsureFile(file);
				f.Open();
				f.Close();
			}
		}

		[Test]
		public void TestMarmaladeFolder()
		{
			using (var rm = this.Container.Resolve<IResourceManager>())
			{
				var s = new FolderTreeSearch(@"C:\Marmalade\6.2\examples\", "*.group");
				foreach (var file in s)
				{
					Console.WriteLine(file);
					var f = rm.EnsureFile(file);
					f.Open();
					f.Close();
				}
			}
		}

		[Test]
		public void TestMarmaladeFolderForBinary()
		{
			using (var rm = this.Container.Resolve<IResourceManager>())
			{
				var s = new FolderTreeSearch(@"C:\Marmalade\6.2\examples\", "*.group.bin");
				foreach (var file in s)
				{
					Console.WriteLine(file);
					var f = rm.EnsureFile(file);
					f.Open();
					f.Close();
				}
			}
		}

		#endregion
	}
}