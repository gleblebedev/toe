using OpenTK;

namespace Toe.Utils.Mesh.Bsp
{
	public struct BspVsdTreeNode
	{
		// Axis aligned bounding box min.

		// Axis aligned bounding box max.

		#region Constants and Fields

		public float D;

		public Vector3 Max;

		public Vector3 Min;

		// Plane normal
		public Vector3 N;

		// Plane D

		/// <summary>
		/// Index of a node or leaf on the negative side of the plane.
		/// </summary>
		public int NegativeNodeIndex;

		/// <summary>
		/// Index of a node or leaf on the positive side of the plane.
		/// </summary>
		public int PositiveNodeIndex;

		#endregion
	}
}