using System;
using System.Collections.Generic;

using OpenTK;

using Toe.Marmalade.IwGraphics.TangentSpace;
using Toe.Resources;
using Toe.Utils;
using Toe.Utils.Mesh;

namespace Toe.Marmalade.IwGraphics
{
	public class ModelBlockPrimBase : Surface
	{
		#region Constants and Fields

		public static new readonly uint TypeHash = Hash.Get("CIwModelBlockPrimBase");

		private MeshStream<ComplexIndex> indices = new MeshStream<ComplexIndex>();

		#endregion

		#region Constructors and Destructors

		public ModelBlockPrimBase(IResourceManager resourceManager)
			: base(resourceManager)
		{
		}

		#endregion

		#region Public Properties

		public override uint ClassHashCode
		{
			get
			{
				return TypeHash;
			}
		}

		public override int Count
		{
			get
			{
				return this.indices.Count;
			}
		}

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

		#endregion

		#region Public Methods and Operators

		public override IEnumerator<int> GetEnumerator()
		{
			int i = 0;
			foreach (var index in this.indices)
			{
				yield return i;
				++i;
			}
		}

		#endregion

		#region Methods

		internal override void CalculateTangents(TangentMixer t, TangentMixer b)
		{
			switch (this.VertexSourceType)
			{
				case VertexSourceType.QuadList:
					this.CalculateTangentsQuadList(t, b);
					return;
				case VertexSourceType.TrianleList:
					this.CalculateTangentsTrianleList(t, b);
					return;
				case VertexSourceType.TrianleStrip:
					this.CalculateTangentsTrianleStrip(t, b);
					return;
			}
		}

		private void CalculateTangentsQuadList(TangentMixer t, TangentMixer b)
		{
			throw new NotImplementedException();
		}

		private void CalculateTangentsTrianleList(TangentMixer t, TangentMixer b)
		{
			for (int i = 0; i < this.indices.Count; i += 3)
			{
				Vector3 p2 = this.Mesh.Vertices[this.indices[i + 2].Vertex];
				Vector3 p1 = this.Mesh.Vertices[this.indices[i + 1].Vertex];
				Vector3 p0 = this.Mesh.Vertices[this.indices[i].Vertex];

				Vector3 n2 = this.Mesh.Normals[this.indices[i + 2].Normal];
				Vector3 n1 = this.Mesh.Normals[this.indices[i + 1].Normal];
				Vector3 n0 = this.Mesh.Normals[this.indices[i].Normal];

				Vector3 v1 = p1 - p0;
				Vector3 v2 = p2 - p0;

				var pu0 = this.Mesh.UV0[this.indices[i].UV0];
				var pu1 = this.Mesh.UV0[this.indices[i + 1].UV0];
				var pu2 = this.Mesh.UV0[this.indices[i + 2].UV0];

				var u1 = pu1 - pu0;
				var u2 = pu2 - pu0;

				var det = u1.X * u2.Y - u2.X * u1.Y;
				if (det == 0)
				{
					det = 1;
				}
				det = 1 / det;

				var tangent =
					Vector3.Normalize(
						new Vector3(
							(v1.X * u2.Y - v2.X * u1.Y) * det, (v1.Y * u2.Y - v2.Y * u1.Y) * det, (v1.Z * u2.Y - v2.Z * u1.Y) * det));
				var bitangent =
					Vector3.Normalize(
						new Vector3(
							(-v1.X * u2.X + v2.X * u1.X) * det, (-v1.Y * u2.X + v2.Y * u1.X) * det, (-v1.Z * u2.X + v2.Z * u1.X) * det));

				var key0 = new TangentKey(p0, pu0);
				var key1 = new TangentKey(p1, pu1);
				var key2 = new TangentKey(p2, pu2);

				{
					ModifyAtFunc<ComplexIndex> modifyT = (ref ComplexIndex a) =>
						{
							a.Tangent = t.Add(key0, ref n0, tangent);
							a.Binormal = b.Add(key0, ref n0, bitangent);
						};
					this.indices.ModifyAt(i, modifyT);
				}
				{
					ModifyAtFunc<ComplexIndex> modifyT = (ref ComplexIndex a) =>
						{
							a.Tangent = t.Add(key1, ref n1, tangent);
							a.Binormal = b.Add(key1, ref n1, bitangent);
						};
					this.indices.ModifyAt(i + 1, modifyT);
				}
				{
					ModifyAtFunc<ComplexIndex> modifyT = (ref ComplexIndex a) =>
						{
							a.Tangent = t.Add(key2, ref n2, tangent);
							a.Binormal = b.Add(key2, ref n2, bitangent);
						};
					this.indices.ModifyAt(i + 2, modifyT);
				}
			}
		}

		private void CalculateTangentsTrianleStrip(TangentMixer t, TangentMixer b)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}