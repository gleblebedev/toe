using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics.OpenGL;

#if WINDOWS_PHONE
using Microsoft.Xna.Framework;
#else

#endif

namespace Toe.Utils.Mesh
{
	public class StreamSubmesh :BaseSubmesh, ISubMesh
	{
		private readonly StreamMesh mesh;

		internal StreamSubmesh(StreamMesh mesh)
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
		public override void RenderOpenGL()
		{
			GL.Begin(BeginMode.Triangles);
			GL.Color3(1.0f, 1.0f, 1.0f);
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

		

		#endregion
	}
}