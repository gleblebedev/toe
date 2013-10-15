using Toe.Utils.ToeMath;

namespace Toe.Utils.Mesh.Bsp
{
	public struct BspVsdTreeLeaf
	{
		// Axis aligned bounding box min.

		#region Constants and Fields

		public int Cluster;

		public Float3 Max;

		public Float3 Min;

		#endregion
	}
}