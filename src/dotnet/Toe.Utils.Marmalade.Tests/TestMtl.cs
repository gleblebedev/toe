using System;
using System.IO;

using NUnit.Framework;

using Toe.Utils.Mesh.Marmalade.IwGx;
using Toe.Utils.Mesh.Marmalade.IwResManager;

namespace Toe.Utils.Mesh.Marmalade.Tests
{
	[TestFixture]
	public class TestMtl
	{
		#region Public Methods and Operators

		[Test]
		public void TestMarmaladeFolder()
		{
			var r = new TextResourceReader();

			var s = new FolderTreeSearch(@"C:\Marmalade\6.2\examples\", "*.mtl");
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