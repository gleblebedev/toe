using OpenTK;

namespace Toe.Utils.Mesh.Bsp
{
	public struct BspVsdTreeNode
	{
		// Axis aligned bounding box min.
		public Vector3 Min;

		// Axis aligned bounding box max.
		public Vector3 Max;

		// Plane normal
		public Vector3 N;

		// Plane D
		public float D;

		/// <summary>
		/// Index of a node or leaf on the positive side of the plane.
		/// </summary>
		public int PositiveNodeIndex;

		/// <summary>
		/// Index of a node or leaf on the negative side of the plane.
		/// </summary>
		public int NegativeNodeIndex;
	}
}