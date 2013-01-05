using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Toe.Resources;

#if WINDOWS_PHONE
using Microsoft.Xna.Framework;
#else

#endif

namespace Toe.Utils.Mesh
{
	public class StreamSubmesh : ISubMesh
	{
		private readonly StreamMesh mesh;

		public StreamSubmesh(StreamMesh mesh)
		{
			this.mesh = mesh;
		}

		private readonly List<StreamSubmeshTriangle> tris = new List<StreamSubmeshTriangle>();

		public List<StreamSubmeshTriangle> Tris
		{
			get
			{
				return this.tris;
			}
		}

		#region Implementation of ISubMesh

#if WINDOWS_PHONE
#else
		public void RenderOpenGL()
		{
			GL.Begin(BeginMode.Triangles);
			foreach (var streamSubmeshTiangle in this.Tris)
			{
				this.RenderVertex(streamSubmeshTiangle.A);
				this.RenderVertex(streamSubmeshTiangle.B);
				this.RenderVertex(streamSubmeshTiangle.C);
			}
			GL.End();
		}

		private void RenderVertex(StreamSubmeshTriangleIndexes indexes)
		{
			if (indexes.Normal >= 0 && this.mesh.Normals.Count > 0)
			{
				GL.Normal3(this.mesh.Normals[indexes.Normal]);
			}
			if (indexes.UV0 >= 0 && this.mesh.UV.Count > 0)
			{
				MeshStream<Vector2> meshStream = this.mesh.UV[0];
				if (meshStream != null && meshStream.Count > 0)
				{
					var vector2 = meshStream[indexes.UV0];
					var v = vector2; // new Vector2(vector2.X, -vector2.Y);
					GL.MultiTexCoord2(TextureUnit.Texture0, ref v);
				}
			}
			if (indexes.UV1 >= 0 && this.mesh.UV.Count > 1)
			{
				MeshStream<Vector2> meshStream = this.mesh.UV[1];
				if (meshStream != null && meshStream.Count > 0)
				{
					var vector2 = meshStream[indexes.UV1];
					GL.MultiTexCoord2(TextureUnit.Texture1, ref vector2);
				}
			}
			if (indexes.Color >= 0 && this.mesh.Colors != null && this.mesh.Colors.Count > 0)
			{
				GL.Color4(this.mesh.Colors[indexes.Color]);
			}
			GL.Vertex3(this.mesh.Vertices[indexes.Vertex]);
		}
#endif

		private string material;

		public string Material
		{
			get
			{
				return this.material;
			}
			set
			{
				if (this.material != value)
				{
					this.material = value;
					this.materialHash = Hash.Get(value);
				}
			}
		}

		private string name;

		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (this.name != value)
				{
					this.name = value;
				}
			}
		}

		private uint materialHash;

		public uint MaterialHash
		{
			get
			{
				return this.materialHash;
			}
			set
			{
				this.materialHash = value;
				this.material = null;
			}
		}

		#endregion
	}
}