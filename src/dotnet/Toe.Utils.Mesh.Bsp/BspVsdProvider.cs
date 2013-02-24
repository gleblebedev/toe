using System;
using System.Collections.Generic;

using OpenTK;

namespace Toe.Utils.Mesh.Bsp
{
	public class BspVsdProvider : ClassWithNotification, IVsdProvider
	{
		#region Constants and Fields

		/// <summary>
		/// Camera position.
		/// </summary>
		protected static PropertyEventArgs CameraPositionEventArgs =
			Expr.PropertyEventArgs<BspVsdProvider>(x => x.CameraPosition);

		private int[] cameraLeaf;

		private Vector3 cameraPosition;

		private BspVsdTreeCluster[] clusters;

		#endregion

		#region Public Properties

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
					this.SelectCameraLeaf();
					this.RaisePropertyChanged(CameraPositionEventArgs.Changed);
				}
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

		public BspVsdTreeLeaf[] Leaves { get; set; }

		/// <summary>
		/// Main mesh, similar to the level geometry.
		/// </summary>
		public IMesh Level { get; set; }

		public BspVsdTreeModel[] Models { get; set; }

		public BspVsdTreeNode[] Nodes { get; set; }

		public int[] VisibleClustersLookupTable { get; set; }

		public int[] VisibleMeshesLookupTable { get; set; }

		#endregion

		#region Public Methods and Operators

		public int GetLeaf(ref Vector3 pos, int model)
		{
			int nodeId = this.Models[model].RootNode;

			float side;
			for (; nodeId >= 0;)
			{
				Vector3.Dot(ref pos, ref this.Nodes[nodeId].N, out side);
				side -= this.Nodes[nodeId].D;
				if (side > 0)
				{
					nodeId = this.Nodes[nodeId].PositiveNodeIndex;
				}
				else
				{
					nodeId = this.Nodes[nodeId].NegativeNodeIndex;
				}
			}
			return -1 - nodeId;
		}

		public IEnumerable<ISubMesh> GetVisibleSubMeshes()
		{
			for (int index = 0; index < this.Models.Length; index++)
			{
				foreach (var p in this.GetVisibleSubmeshesAtModel(index))
				{
					yield return p;
				}
			}
		}

		public void SelectCameraLeaf()
		{
			if (this.cameraLeaf == null || this.cameraLeaf.Length < this.Models.Length)
			{
				this.cameraLeaf = new int[this.Models.Length];
			}

			for (int index = 0; index < this.Models.Length; index++)
			{
				this.cameraLeaf[index] = this.GetLeaf(ref this.cameraPosition, index);
			}
		}

		#endregion

		#region Methods

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
			if (index0 < 0 || index0 >= this.clusters.Length)
			{
				throw new IndexOutOfRangeException(string.Format("Index {0} is not a valid index for cluster collection.", index0));
			}
			var bspVsdTreeCluster = this.Clusters[index0];
			var offset = bspVsdTreeCluster.VisibleMeshesOffset;
			var end = offset + bspVsdTreeCluster.VisibleMeshesCount;

			for (int i = offset; i < end; i++)
			{
				yield return this.Level.Submeshes[i];
			}
		}

		#endregion
	}
}