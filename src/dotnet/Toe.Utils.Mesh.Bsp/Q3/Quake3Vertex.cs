using System.Drawing;

using OpenTK;

namespace Toe.Utils.Mesh.Bsp.Q3
{
	public struct Quake3Vertex
	{
		#region Constants and Fields

		public Color color; // RGBA color for the vertex [4]

		public Vector2 vLightmapCoord; // (u, v) lightmap coordinate

		public Vector3 vNormal; // (x, y, z) normal vector

		public Vector3 vPosition; // (x, y, z) position. 

		public Vector2 vTextureCoord; // (u, v) texture coordinate

		#endregion
	}
}