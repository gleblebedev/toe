using System;

using Autofac;

using NUnit.Framework;

using Toe.Resources;

namespace Toe.Utils.Mesh.Marmalade.Tests
{
	[TestFixture]
	public class TestSkin : BaseTest
	{
		#region Public Methods and Operators

		[Test]
		public void TestLegs()
		{
			using (IResourceManager rm = this.Container.Resolve<IResourceManager>())
			{
				var fileName = "male_legs_trousers0_lod0.skin";
				var f = rm.EnsureFile(fileName);
				f.Open();
				Assert.Greater(f.Items.Count, 0);
				f.Close();
			}
		}

		[Test]
		public void TestMarmaladeFolder()
		{
			using (IResourceManager rm = this.Container.Resolve<IResourceManager>())
			{
				var s = new FolderTreeSearch(@"C:\Marmalade\6.2\examples\", "*.skin");
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