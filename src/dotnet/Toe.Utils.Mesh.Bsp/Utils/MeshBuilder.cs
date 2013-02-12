using System.Collections.Generic;

namespace Toe.Utils.Mesh.Bsp.Utils
{
	internal struct MeshBuilderMesh
	{
		public int Index;

		public VertexBufferSubmesh Mesh;
	}
	internal class MeshBuilder
	{
		private readonly VertexBufferMesh streamMesh;

		private readonly IMaterialProvider materialProvider;

		Dictionary<BspSubmeshKey, MeshBuilderMesh> map = new Dictionary<BspSubmeshKey, MeshBuilderMesh>();

		private Dictionary<BspMaterialKey, IMaterial> mapMtl = new Dictionary<BspMaterialKey, IMaterial>();
		public MeshBuilder(VertexBufferMesh streamMesh, IMaterialProvider materialProvider)
		{
			this.streamMesh = streamMesh;
			this.materialProvider = materialProvider;
		}

		public VertexBufferSubmesh EnsureSubMesh(BspSubmeshKey bspSubmeshKey, out int meshIndex)
		{
			MeshBuilderMesh m;
			if (map.TryGetValue(bspSubmeshKey, out m))
			{
				meshIndex = m.Index;
				return m.Mesh;
			}
			m.Index = streamMesh.Submeshes.Count;
			m.Mesh = (VertexBufferSubmesh)this.streamMesh.CreateSubmesh();
			map[bspSubmeshKey] = m;
			meshIndex = m.Index;

			IMaterial mtl;
			if (!mapMtl.TryGetValue(bspSubmeshKey.Material, out mtl))
			{
				mtl = materialProvider.CreateMaterial(bspSubmeshKey.Material);
				mapMtl.Add(bspSubmeshKey.Material,mtl);
			}
			m.Mesh.Material = mtl;
			return m.Mesh;
		}
	}
}