using System.Drawing;

using Toe.Utils.ToeMath;

namespace Toe.Utils.Mesh.Bsp
{
	public class BspMeshStreams
	{
		public ListMeshStream<Float3> Positions;
		public ListMeshStream<Float3> Normals;
		public ListMeshStream<Float2> TexCoord0;
		public ListMeshStream<Float2> TexCoord1;
		public ListMeshStream<Color> Colors;
	}
}