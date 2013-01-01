using System;
using System.IO;

using Autofac;

using NUnit.Framework;

using Toe.Resources;

namespace Toe.Utils.Mesh.Marmalade.Tests
{
	[TestFixture]
	public class TestMtl:BaseTest
	{
		#region Public Methods and Operators

		[Test]
		public void TestMarmaladeFolder()
		{
			using (IResourceManager rm = Container.Resolve<IResourceManager>())
			{
				var s = new FolderTreeSearch(@"C:\Marmalade\6.2\examples\", "*.mtl");
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