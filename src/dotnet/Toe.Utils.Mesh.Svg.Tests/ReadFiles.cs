using System;
using System.IO;

using NUnit.Framework;

namespace Toe.Utils.Mesh.Svg.Tests
{
	[TestFixture]
	public class StandartTests
	{
		#region Public Methods and Operators

		[Test]
		public void StandartTestsFolder()
		{
			var s = new FolderTreeSearch(@"\\vmware-host\Shared Folders\Shared\SVG", "*.svg");
			var fab = new SvgSceneFileFormat();
			foreach (var file in s)
			{
				Console.WriteLine(file);
				using (var f = File.OpenRead(file))
				{
					var r = fab.CreateReader();
					r.Load(f, Path.GetDirectoryName(file));
				}
			}
		}

		#endregion
	}
}