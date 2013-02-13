using OpenTK;

namespace Toe.Utils.Mesh.Bsp
{
	public struct BspVsdTreeModel
	{
		public int RootNode;
	}
	public struct BspVsdTreeCluster
	{
		public int VisibleClustersOffset;
		public int VisibleClustersCount;
		public int VisibleMeshesOffset;
		public int VisibleMeshesCount;
	}
	public struct BspVsdTreeLeaf
	{
		// Axis aligned bounding box min.
		public Vector3 Min;

		// Axis aligned bounding box max.
		public Vector3 Max;

		public int Cluster;
	}
}