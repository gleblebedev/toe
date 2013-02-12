using OpenTK;

namespace Toe.Utils.Mesh.Bsp
{
	public struct BspVsdTreeLeaf
	{
		// Axis aligned bounding box min.
		public Vector3 Min;

		// Axis aligned bounding box max.
		public Vector3 Max;

		public int VisibleLeafsOffset;
		public int VisibleLeafsCount;
		public int VisibleMeshesOffset;
		public int VisibleMeshesCount;
	}
}