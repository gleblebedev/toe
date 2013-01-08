using System;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Mesh as set of submeshes with common vertex buffer.
	/// 
	/// The implemenation is not efficient!
	/// Please use it in content pipeline only! It is NOT recommended to use it in production.
	/// </summary>
	public class VertexBufferMesh : IMesh
	{
		#region Constants and Fields

		private readonly OptimizedList<Vertex> vertexBuffer = new OptimizedList<Vertex>();

		#endregion

		#region Public Properties

		private readonly List<ISubMesh> submeshes = new List<ISubMesh>();

		public string Name { get; set; }

		public uint NameHash { get; set; }

		public IList<ISubMesh> Submeshes
		{
			get
			{
				return submeshes;
			}
		}

		public OptimizedList<Vertex> VertexBuffer
		{
			get
			{
				return this.vertexBuffer;
			}
		}

		#endregion

		#region Public Methods and Operators

		public ISubMesh CreateSubmesh()
		{
			var streamSubmesh = new VertexBufferSubmesh(this);
			this.Submeshes.Add(streamSubmesh);
			return streamSubmesh;
		}

		public void RenderOpenGL()
		{
			var subMeshes = this.submeshes;
			foreach (ISubMesh subMesh in subMeshes)
			{
				subMesh.RenderOpenGL();
			}
		}

		#endregion

		public void RenderOpenGLVertex(int index)
		{
			GL.Normal3(vertexBuffer[index].Normal);
			Vector3 vector3 = vertexBuffer[index].UV0;
			GL.MultiTexCoord3(TextureUnit.Texture0, ref vector3);
			vector3 = vertexBuffer[index].UV1;
			GL.MultiTexCoord3(TextureUnit.Texture1, ref vector3);
			GL.Vertex3(vertexBuffer[index].Position);
		}
	}
}