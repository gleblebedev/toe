using System.Collections.Generic;

using OpenTK;

namespace Toe.Utils.Mesh.Bsp
{
	public class BspVsdProvider:ClassWithNotification, IVsdProvider
	{
		public BspVsdProvider(BspVsdTreeNode[] nodes, BspVsdTreeLeaf[] leaves, int[] visibleLeavesLookupTable, int[] visibleMeshesLookupTable)
		{
			this.nodes = nodes;
			this.leaves = leaves;
			this.visibleLeavesLookupTable = visibleLeavesLookupTable;
			this.visibleMeshesLookupTable = visibleMeshesLookupTable;
		}

		private BspVsdTreeNode[] nodes;

		private readonly BspVsdTreeLeaf[] leaves;

		private readonly int[] visibleLeavesLookupTable;

		private readonly int[] visibleMeshesLookupTable;

		#region Implementation of IVsdProvider

		/// <summary>
		/// Main mesh, similar to the level geometry.
		/// </summary>
		public IMesh Level { get; set; }

		/// <summary>
		/// Camera position.
		/// </summary>
		protected static PropertyEventArgs CameraPositionEventArgs = Expr.PropertyEventArgs<BspVsdProvider>(x => x.CameraPosition);

		private Vector3 cameraPosition;

		private int cameraLeaf;

		public Vector3 CameraPosition
		{
			get
			{
				return this.cameraPosition;
			}
			set
			{
				if (this.cameraPosition != value)
				{
					this.RaisePropertyChanging(CameraPositionEventArgs.Changing);
					this.cameraPosition = value;
					SelectCameraLeaf();
					this.RaisePropertyChanged(CameraPositionEventArgs.Changed);
				}
			}
		}
		public int GetLeaf(ref Vector3 pos)
		{
			int nodeId = 0;

			float side;
			for (; ; )
			{
				Vector3.Dot(ref pos, ref nodes[nodeId].N, out side);
				side -= nodes[nodeId].D;
				if (side > 0) nodeId = nodes[nodeId].PositiveNodeIndex;
				else nodeId = nodes[nodeId].NegativeNodeIndex;
				if (nodeId < 0) return -1 - nodeId;
			}
		}
		public void SelectCameraLeaf()
		{
			cameraLeaf = this.GetLeaf(ref cameraPosition);
		}

		public IEnumerable<ISubMesh> GetVisibleSubMeshes()
		{
			var index0 = cameraLeaf;
			foreach (var p in this.VisitLeafMeshes(index0)) yield return p;

			var begin = this.leaves[index0].VisibleLeafsOffset;
			var end = begin + this.leaves[index0].VisibleLeafsCount;
			for (int i = begin; i < end; i++)
			{
				foreach (var p in this.VisitLeafMeshes(this.visibleLeavesLookupTable[i])) yield return p;
			}
			
		}

		private IEnumerable<ISubMesh> VisitLeafMeshes(int index0)
		{
			var offset = this.leaves[index0].VisibleMeshesOffset;
			var end = offset + this.leaves[index0].VisibleMeshesCount;
			for (int i = offset; i < end; i++)
			{
				yield return Level.Submeshes[i];
			}
		}

		#endregion
	}
}