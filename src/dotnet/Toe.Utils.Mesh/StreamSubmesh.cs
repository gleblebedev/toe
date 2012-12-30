using System.Collections.Generic;

using OpenTK.Graphics.OpenGL;
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

		private readonly List<StreamSubmeshTiangle> tris = new List<StreamSubmeshTiangle>();

		public List<StreamSubmeshTiangle> Tris
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

		private void RenderVertex(StreamSubmeshTiangleIndexes indexes)
		{
			if (indexes.Normal >= 0)
			{
				GL.Normal3(this.mesh.Normals[indexes.Normal]);
			}
			if (indexes.UV0 >= 0)
			{
				GL.TexCoord2(this.mesh.UV[0][indexes.UV0]);
			}
			if (indexes.Color >= 0)
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
				}
			}
		}

		#endregion
	}
}