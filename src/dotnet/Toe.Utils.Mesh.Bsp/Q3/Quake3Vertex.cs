using System.Drawing;

using Toe.Utils.ToeMath;

namespace Toe.Utils.Mesh.Bsp.Q3
{
	public struct Quake3Vertex
	{
		#region Constants and Fields

		public Color color; // RGBA color for the vertex [4]

		public Float2 vLightmapCoord; // (u, v) lightmap coordinate

		public Float3 vNormal; // (x, y, z) normal vector

		public Float3 vPosition; // (x, y, z) position. 

		public Float2 vTextureCoord; // (u, v) texture coordinate

		#endregion
	}
}