using System.Collections.Generic;

using OpenTK;

namespace Toe.Utils.Mesh.Bsp.HL2
{
	public struct SourcePlane
	{
		public Vector3 normal;     // normal vector
		public float dist;       // distance from origin
		public int type;       // plane axis identifier
	}
}