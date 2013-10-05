using System.Drawing;

using OpenTK;

namespace Toe.Utils.Mesh.Bsp
{
	public class BspMeshStreams
	{
		public ListMeshStream<Vector3> Positions;
		public ListMeshStream<Vector3> Normals;
		public ListMeshStream<Vector2> TexCoord0;
		public ListMeshStream<Vector2> TexCoord1;
		public ListMeshStream<Color> Colors;
	}
}