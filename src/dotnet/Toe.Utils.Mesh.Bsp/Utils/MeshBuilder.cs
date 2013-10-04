using System.Collections.Generic;

namespace Toe.Utils.Mesh.Bsp.Utils
{
	internal struct MeshBuilderMesh
	{
		#region Constants and Fields

		public int Index;

		public VertexBufferSubmesh<Vertex> Mesh;

		#endregion
	}

	internal class MeshBuilder
	{
		#region Constants and Fields

		private readonly Dictionary<BspSubmeshKey, MeshBuilderMesh> map = new Dictionary<BspSubmeshKey, MeshBuilderMesh>();

		private readonly Dictionary<BspMaterialKey, IMaterial> mapMtl = new Dictionary<BspMaterialKey, IMaterial>();

		private readonly IMaterialProvider materialProvider;

		private readonly VertexBufferMesh<Vertex> streamMesh;

		#endregion

		#region Constructors and Destructors

		public MeshBuilder(VertexBufferMesh<Vertex> streamMesh, IMaterialProvider materialProvider)
		{
			this.streamMesh = streamMesh;
			this.materialProvider = materialProvider;
		}

		#endregion

		#region Public Methods and Operators

		public VertexBufferSubmesh<Vertex> EnsureSubMesh(BspSubmeshKey bspSubmeshKey, out int meshIndex)
		{
			MeshBuilderMesh m;
			if (this.map.TryGetValue(bspSubmeshKey, out m))
			{
				meshIndex = m.Index;
				return m.Mesh;
			}
			m.Index = this.streamMesh.Submeshes.Count;
			m.Mesh = (VertexBufferSubmesh<Vertex>)this.streamMesh.CreateSubmesh();
			this.map[bspSubmeshKey] = m;
			meshIndex = m.Index;

			IMaterial mtl;
			if (!this.mapMtl.TryGetValue(bspSubmeshKey.Material, out mtl))
			{
				mtl = this.materialProvider.CreateMaterial(bspSubmeshKey.Material);
				this.mapMtl.Add(bspSubmeshKey.Material, mtl);
			}
			m.Mesh.Material = mtl;
			return m.Mesh;
		}

		#endregion
	}
}