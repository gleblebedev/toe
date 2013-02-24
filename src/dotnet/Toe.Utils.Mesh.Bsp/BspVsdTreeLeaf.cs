using OpenTK;

namespace Toe.Utils.Mesh.Bsp
{
	public struct BspVsdTreeLeaf
	{
		// Axis aligned bounding box min.

		#region Constants and Fields

		public int Cluster;

		public Vector3 Max;

		public Vector3 Min;

		#endregion
	}
}