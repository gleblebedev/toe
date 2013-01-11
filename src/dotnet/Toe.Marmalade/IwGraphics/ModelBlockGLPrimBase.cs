using System.Collections.Generic;

using OpenTK;

using Toe.Resources;
using Toe.Utils.Mesh;

namespace Toe.Marmalade.IwGraphics
{
	public class ModelBlockGLPrimBase : Surface
	{
		public static new readonly uint TypeHash = Hash.Get("CIwModelBlockGLPrimBase");

		private readonly MeshStream<int> indices = new MeshStream<int>();

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
				var v = new Vertex();
				if (IsVertexStreamAvailable) v.Position = Mesh.Vertices[index];
				if (IsColorStreamAvailable) v.Color = Mesh.Colors[index];
				if (IsNormalStreamAvailable) v.Normal = Mesh.Normals[index];
				if (IsUV0StreamAvailable)
				{
					Vector2 uv0 = Mesh.UV0[index];
					v.UV0 = new Vector3(uv0.X,uv0.Y,0);
				}
				if (IsUV1StreamAvailable)
				{
					Vector2 uv1 = Mesh.UV1[index];
					v.UV0 = new Vector3(uv1.X, uv1.Y, 0);
				}
				yield return v;
			}
		}

		public MeshStream<int> Indices
		{
			get
			{
				return this.indices;
			}
		}

		public ModelBlockGLPrimBase(IResourceManager resourceManager)
			: base(resourceManager)
		{
		}
	}
}