using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics.OpenGL;

#if WINDOWS_PHONE
using Microsoft.Xna.Framework;
#else

#endif

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Mesh as set of submeshes with common set of data streams.
	/// 
	/// The implemenation is not efficient!
	/// Please use it in content pipeline only! It is NOT recommended to use it in production.
	/// </summary>
	public class StreamSubmesh :BaseSubmesh, ISubMesh
	{
		private readonly StreamMesh mesh;

		internal StreamSubmesh(StreamMesh mesh)
		{
			this.mesh = mesh;
		}
		public object RenderData
		{
			get;
			set;
		}
		private readonly List<StreamSubmeshIndexes> indices = new List<StreamSubmeshIndexes>();

		public List<StreamSubmeshIndexes> Indices
		{
			get
			{
				return this.indices;
			}
		}

		#region Implementation of ISubMesh

#if WINDOWS_PHONE
#else
		//public override void RenderOpenGL()
		//{
		//    GL.Begin(BeginMode.Triangles);
		//    GL.Color3(1.0f, 1.0f, 1.0f);
		//    foreach (var streamSubmeshTiangle in this.Tris)
		//    {
		//        this.RenderVertex(streamSubmeshTiangle.A);
		//        this.RenderVertex(streamSubmeshTiangle.B);
		//        this.RenderVertex(streamSubmeshTiangle.C);
		//    }
		//    GL.End();
		//}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public override IEnumerator<int> GetEnumerator()
		{
			int j=0;
			foreach (var i in Indices)
			{
				yield return j;
				++j;
			}
		}

		public override int Count
		{
			get
			{
				return Indices.Count;
			}
		}

		public override VertexSourceType VertexSourceType
		{
			get
			{
				return VertexSourceType.TrianleList;
			}
		}

		//private void RenderVertex(StreamSubmeshIndexes indexes)
		//{
		//    if (indexes.Normal >= 0 && this.mesh.Normals.Count > 0)
		//    {
		//        GL.Normal3(this.mesh.Normals[indexes.Normal]);
		//    }
		//    if (indexes.UV0 >= 0 && this.mesh.UV.Count > 0)
		//    {
		//        MeshStream<Vector2> meshStream = this.mesh.UV[0];
		//        if (meshStream != null && meshStream.Count > 0)
		//        {
		//            var vector2 = meshStream[indexes.UV0];
		//            var v = vector2; // new Vector2(vector2.X, -vector2.Y);
		//            GL.MultiTexCoord2(TextureUnit.Texture0, ref v);
		//        }
		//    }
		//    if (indexes.UV1 >= 0 && this.mesh.UV.Count > 1)
		//    {
		//        MeshStream<Vector2> meshStream = this.mesh.UV[1];
		//        if (meshStream != null && meshStream.Count > 0)
		//        {
		//            var vector2 = meshStream[indexes.UV1];
		//            GL.MultiTexCoord2(TextureUnit.Texture1, ref vector2);
		//        }
		//    }
		//    if (indexes.Color >= 0 && this.mesh.Colors != null && this.mesh.Colors.Count > 0)
		//    {
		//        GL.Color4(this.mesh.Colors[indexes.Color]);
		//    }
		//    GL.Vertex3(this.mesh.Vertices[indexes.Vertex]);
		//}
#endif

		

		#endregion
	}
}