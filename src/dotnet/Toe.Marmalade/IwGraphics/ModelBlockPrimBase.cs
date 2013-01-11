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

		public override IEnumerator<Vertex> GetEnumerator()
		{
			foreach (var index in indices)
			{
				var vertex = new Vertex();
				if (Mesh.IsVertexStreamAvailable) vertex.Position = Mesh.Vertices[index.Vertex];
				if (Mesh.IsNormalStreamAvailable) vertex.Normal = Mesh.Normals[index.Normal];
				if (Mesh.IsColorStreamAvailable) vertex.Color = Mesh.Colors[index.Color];
				if (Mesh.IsUV0StreamAvailable)
				{
					Vector2 vector2 = Mesh.UV0[index.UV0];
					vertex.UV0 = new Vector3(vector2.X, vector2.Y, 0);
				}
				if (Mesh.IsUV1StreamAvailable)
				{
					Vector2 vector2 = Mesh.UV0[index.UV1];
					vertex.UV1 = new Vector3(vector2.X, vector2.Y, 0);
				}
				//if (Mesh.IsTangentStreamAvailable) vertex.Tangent = Mesh.Tangents[index.Tangent];
				//if (Mesh.IsBinormalStreamAvailable) vertex.Binormal = Mesh.Binormals[index.Binormal];
				yield return vertex;
			}
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