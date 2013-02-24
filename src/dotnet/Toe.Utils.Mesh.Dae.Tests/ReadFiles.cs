using System.IO;

using NUnit.Framework;

namespace Toe.Utils.Mesh.Dae.Tests
{
	[TestFixture]
	public class ReadFiles
	{
		[Test]
		public void LemarchandsBox()
		{
			var r = new DaeReader();
			using (var s = File.OpenRead("lemarchandsbox.DAE"))
			{
				var scene = r.Load(s, null);
			}
		}
	}
}