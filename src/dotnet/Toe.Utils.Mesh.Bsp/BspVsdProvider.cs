using System;
using System.Collections.Generic;

using OpenTK;

namespace Toe.Utils.Mesh.Bsp
{
	public class BspVsdProvider:ClassWithNotification, IVsdProvider
	{
		public BspVsdProvider()
		{
		}

		private BspVsdTreeNode[] nodes;

		private BspVsdTreeLeaf[] leaves;
		private BspVsdTreeCluster[] clusters;

		private int[] visibleClustersLookupTable;

		private int[] visibleMeshesLookupTable;

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

		private int[] cameraLeaf;

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

		public BspVsdTreeNode[] Nodes
		{
			get
			{
				return this.nodes;
			}
			set
			{
				this.nodes = value;
			}
		}

	

		public BspVsdTreeLeaf[] Leaves
		{
			get
			{
				return this.leaves;
			}
			set
			{
				this.leaves = value;
			}
		}

		public BspVsdTreeCluster[] Clusters
		{
			get
			{
				return this.clusters;
			}
			set
			{
				this.clusters = value;
			}
		}

		public int[] VisibleClustersLookupTable
		{
			get
			{
				return this.visibleClustersLookupTable;
			}
			set
			{
				this.visibleClustersLookupTable = value;
			}
		}

		public int[] VisibleMeshesLookupTable
		{
			get
			{
				return this.visibleMeshesLookupTable;
			}
			set
			{
				this.visibleMeshesLookupTable = value;
			}
		}

		public BspVsdTreeModel[] Models { get; set; }

		public int GetLeaf(ref Vector3 pos, int model)
		{
			int nodeId = this.Models[model].RootNode;

			float side;
			for (; nodeId >=0; )
			{
				Vector3.Dot(ref pos, ref this.Nodes[nodeId].N, out side);
				side -= this.Nodes[nodeId].D;
				if (side > 0) nodeId = this.Nodes[nodeId].PositiveNodeIndex;
				else nodeId = this.Nodes[nodeId].NegativeNodeIndex;
			}
			return -1 - nodeId;
		}
		public void SelectCameraLeaf()
		{
			if (cameraLeaf == null || cameraLeaf.Length < Models.Length)
				cameraLeaf = new int[Models.Length];

			for (int index = 0; index < this.Models.Length; index++)
			{
				this.cameraLeaf[index] = this.GetLeaf(ref cameraPosition,index);
			}
		}

		public IEnumerable<ISubMesh> GetVisibleSubMeshes()
		{
			for (int index = 0; index < this.Models.Length; index++)
			{
				foreach (var p in this.GetVisibleSubmeshesAtModel(index)) yield return p;
			}
		}

		private IEnumerable<ISubMesh> GetVisibleSubmeshesAtModel(int model)
		{
			var index0 = this.Leaves[this.cameraLeaf[model]].Cluster;
			if (index0 < 0)
			{
				//foreach (var p in this.Level.Submeshes)
				//{
				//    yield return p;
				//}
				yield break;
			}
			foreach (var p in this.VisitClusterMeshes(index0))
			{
				yield return p;
			}

			var begin = this.Clusters[index0].VisibleClustersOffset;
			var end = begin + this.Clusters[index0].VisibleClustersCount;
			for (int i = begin; i < end; i++)
			{
				foreach (var p in this.VisitClusterMeshes(this.VisibleClustersLookupTable[i]))
				{
					yield return p;
				}
			}
		}

		private IEnumerable<ISubMesh> VisitClusterMeshes(int index0)
		{
			if (index0 <0 || index0 >=this.clusters.Length)
			{
				throw new IndexOutOfRangeException(string.Format("Index {0} is not a valid index for cluster collection.", index0));
			}
			var bspVsdTreeCluster = this.Clusters[index0];
			var offset = bspVsdTreeCluster.VisibleMeshesOffset;
				var end = offset + bspVsdTreeCluster.VisibleMeshesCount;
			
				for (int i = offset; i < end; i++)
				{
					yield return Level.Submeshes[i];
				}
		}

		#endregion
	}
}