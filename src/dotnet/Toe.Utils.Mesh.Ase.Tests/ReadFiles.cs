using System.Diagnostics;
using System.IO;

using NUnit.Framework;

using OpenTK;

using Toe.Utils.ToeMath;

namespace Toe.Utils.Mesh.Ase.Tests
{
	[TestFixture]
	public class ReadFiles
	{
		#region Public Methods and Operators

		[Test]
		public void LemarchandsBox()
		{
			var r = new AseReader(StreamConverterFactory.Default);
			using (var s = File.OpenRead("lemarchandsbox.ASE"))
			{
				var scene = r.Load(s, null);
				foreach (var geometry in scene.Geometries)
				{
					var reader = geometry.GetStreamReader<Float3>(Streams.Position, 0);
					var normalReader = geometry.GetStreamReader<Float3>(Streams.Normal, 0);
					var texCoordReader = geometry.GetStreamReader<Float3>(Streams.TexCoord, 0);
					var colorReader = geometry.GetStreamReader<Float3>(Streams.Color, 0);
					//for (int i = 0; i < geometry.Count; ++i)
					//{
					//	Debug.Write(string.Format("{0} ", reader[i]));
					//	if (normalReader != null)
					//	Debug.Write(string.Format("{0} ", normalReader[i]));
					//	if (texCoordReader != null)
					//	Debug.Write(string.Format("{0} ", texCoordReader[i]));
					//	if (colorReader != null)
					//	Debug.Write(string.Format("{0} ", colorReader[i]));
					//	Debug.WriteLine("");
					//}
					foreach (var submesh in geometry.Submeshes)
					{
						var positionIndex = submesh.GetIndexReader(Streams.Position, 0);
						var normalIndex = submesh.GetIndexReader(Streams.Normal, 0);
						var texCoordIndex = submesh.GetIndexReader(Streams.TexCoord, 0);
						var colorIndex = submesh.GetIndexReader(Streams.Color, 0);
						for (int i=0; i<submesh.Count;++i)
						{
							Debug.Write(string.Format("{0} ", positionIndex[i]));
							Debug.Write(string.Format("{0} ", reader[positionIndex[i]]));
							if (normalReader != null)
							{
								Debug.Write(string.Format("{0} ", normalIndex[i]));
								Debug.Write(string.Format("{0} ", normalReader[normalIndex[i]]));
							}
								if (texCoordReader != null)
							{
								Debug.Write(string.Format("{0} ", texCoordIndex[i]));
								Debug.Write(string.Format("{0} ", texCoordReader[texCoordIndex[i]]));
							}
								if (colorReader != null)
								{
									Debug.Write(string.Format("{0} ", colorIndex[i]));
									Debug.Write(string.Format("{0} ", colorReader[colorIndex[i]]));
								}
							Debug.WriteLine("");
						}
					}
				}
			}
		}

		#endregion
	}
}
