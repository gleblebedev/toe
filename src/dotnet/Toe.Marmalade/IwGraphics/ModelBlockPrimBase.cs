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

		private ListMeshStream<ComplexIndex> indices = new ListMeshStream<ComplexIndex>();

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

		public ListMeshStream<ComplexIndex> Indices
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
		public override IList<int> GetIndexReader(string key, int channel)
		{
			if (key == Streams.Position && channel == 0)
			{
				return new ComplexIndexReader(Indices, x => x.Vertex);
			}
			if (key == Streams.Normal && channel == 0)
			{
				return new ComplexIndexReader(Indices, x => x.Normal);
			}
			if (key == Streams.Binormal && channel == 0)
			{
				return new ComplexIndexReader(Indices, x => x.Binormal);
			}
			if (key == Streams.Tangent && channel == 0)
			{
				return new ComplexIndexReader(Indices, x => x.Tangent);
			}
			if (key == Streams.Color && channel == 0)
			{
				return new ComplexIndexReader(Indices, x => x.Color);
			}
			if (key == Streams.TexCoord && channel == 0)
			{
				return new ComplexIndexReader(Indices, x => x.UV0);
			}
			if (key == Streams.TexCoord && channel == 1)
			{
				return new ComplexIndexReader(Indices, x => x.UV1);
			}
			return null;
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
				case VertexSourceType.TriangleList:
					this.CalculateTangentsTrianleList(t, b);
					return;
				case VertexSourceType.TriangleStrip:
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

				var pu0 = this.Mesh.IsUV0StreamAvailable?this.Mesh.UV0[this.indices[i].UV0]:Vector2.Zero;
				var pu1 = this.Mesh.IsUV0StreamAvailable?this.Mesh.UV0[this.indices[i + 1].UV0]:Vector2.Zero;
				var pu2 = this.Mesh.IsUV0StreamAvailable ? this.Mesh.UV0[this.indices[i + 2].UV0] : Vector2.Zero;

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