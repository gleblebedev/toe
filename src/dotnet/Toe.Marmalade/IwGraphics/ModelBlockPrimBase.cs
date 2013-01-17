using System;
using System.Collections.Generic;

using OpenTK;

using Toe.Resources;
using Toe.Utils.Mesh;

namespace Toe.Marmalade.IwGraphics
{
	public class ModelBlockPrimBase:Surface
	{
		public static new readonly uint TypeHash = Hash.Get("CIwModelBlockPrimBase");

		public override uint ClassHashCode
		{
			get
			{
				return TypeHash;
			}
		}

		public override IEnumerator<int> GetEnumerator()
		{
			int i = 0;
			foreach (var index in indices)
			{
				yield return i;
				++i;
			}
		}
		public override int Count
		{
			get
			{
				return indices.Count;
			}
		}
		internal override void CalculateTangents(OptimizedList<Vector3> t, OptimizedList<Vector3> b)
		{
			switch (VertexSourceType)
			{
				case VertexSourceType.QuadList:
					CalculateTangentsQuadList(t, b);
					return;
				case VertexSourceType.TrianleList:
					CalculateTangentsTrianleList(t, b);
					return;
				case VertexSourceType.TrianleStrip:
					CalculateTangentsTrianleStrip(t, b);
					return;
			}
		}

		private void CalculateTangentsTrianleStrip(OptimizedList<Vector3> t, OptimizedList<Vector3> b)
		{
			throw new NotImplementedException();
		}

		private void CalculateTangentsTrianleList(OptimizedList<Vector3> t, OptimizedList<Vector3> b)
		{
			for (int i=0; i<indices.Count;i+=3)
			{
				Vector3 p2 = Mesh.Vertices[indices[i + 2].Vertex];
				Vector3 p1 = Mesh.Vertices[indices[i + 1].Vertex];
				Vector3 p0 = Mesh.Vertices[indices[i].Vertex];
				Vector3 v1 = p1 - p0;
				Vector3 v2 = p2 - p0;

				var pu0 = Mesh.UV0[indices[i].UV0];
				var pu1 = Mesh.UV0[indices[i+1].UV0];
				var pu2 = Mesh.UV0[indices[i+2].UV0];

				var u1 = pu1 - pu0;
				var u2 = pu2 - pu0;

				var det = u1.X * u2.Y - u2.X * u1.Y;
				if (det == 0) det = 1;
				det = 1 / det;

				var tangent = Vector3.Normalize(new Vector3((v1.X * u2.Y - v2.X * u1.Y) * det, (v1.Y * u2.Y - v2.Y * u1.Y) * det, (v1.Z * u2.Y - v2.Z * u1.Y) * det));
				var bitangent = Vector3.Normalize(new Vector3((-v1.X * u2.X + v2.X * u1.X) * det, (-v1.Y * u2.X + v2.Y * u1.X) * det, (-v1.Z * u2.X + v2.Z * u1.X) * det));

				int tindex = t.Add(tangent);
				int bindex = b.Add(bitangent);
				ModifyAtFunc<ComplexIndex> modifyT = (ref ComplexIndex a) => { a.Tangent = tindex;a.Binormal = bindex;};
				indices.ModifyAt(i, modifyT);
				indices.ModifyAt(i+1, modifyT);
				indices.ModifyAt(i+2, modifyT);
			}
		}

		private void CalculateTangentsQuadList(OptimizedList<Vector3> t, OptimizedList<Vector3> b)
		{
			throw new NotImplementedException();
		}

		private MeshStream<ComplexIndex> indices = new MeshStream<ComplexIndex>();

		public MeshStream<ComplexIndex> Indices
		{
			get
			{
				return this.indices;
			}
			set
			{
				this.indices = value;
			}
		}

		public ModelBlockPrimBase(IResourceManager resourceManager)
			: base(resourceManager)
		{
		}
	}
}