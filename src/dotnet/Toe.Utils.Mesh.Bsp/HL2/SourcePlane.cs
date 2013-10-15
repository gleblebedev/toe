using Toe.Utils.ToeMath;

namespace Toe.Utils.Mesh.Bsp.HL2
{
	public struct SourcePlane
	{
		#region Constants and Fields

		public float dist; // distance from origin

		public Float3 normal; // normal vector

		public int type; // plane axis identifier

		#endregion
	}
}