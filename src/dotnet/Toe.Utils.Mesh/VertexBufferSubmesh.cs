using System.Collections.Generic;

using OpenTK.Graphics.OpenGL;

namespace Toe.Utils.Mesh
{
	public class VertexBufferSubmesh : BaseSubmesh,ISubMesh
	{
		List<int> indices = new List<int>();

		private readonly VertexBufferMesh vertexBufferMesh;

		public VertexBufferSubmesh(VertexBufferMesh vertexBufferMesh)
		{
			this.vertexBufferMesh = vertexBufferMesh;
		}

		#region Implementation of ISubMesh

		public override void RenderOpenGL()
		{
			GL.Begin(BeginMode.Triangles);
			GL.Color3(1.0f, 1.0f, 1.0f);
			foreach (var index in indices)
			{
				vertexBufferMesh.RenderOpenGLVertex(index);
			}
			GL.End();
		}

	
		#endregion

		public void Add(int i)
		{
			indices.Add(i);
		}
		public void Add(ref Vertex v)
		{
			this.Add(vertexBufferMesh.VertexBuffer.Add(v));
		}
	}
}