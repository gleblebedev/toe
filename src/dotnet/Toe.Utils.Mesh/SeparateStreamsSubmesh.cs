using System.Collections.Generic;

using Toe.Utils.ToeMath;

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
	public class SeparateStreamsSubmesh : BaseSubmesh, ISubMesh
	{
		private readonly SeparateStreamsMesh mesh;

		internal SeparateStreamsSubmesh(SeparateStreamsMesh mesh)
		{
			this.mesh = mesh;
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


	

		protected override void CalculateActualBounds()
		{
			var positions = this.mesh.GetStreamReader<Float3>(Streams.Position, 0);
			foreach (var index in this.GetIndexReader(Streams.Position,0))
			{

				var position = positions[index];
				if (this.boundingBoxMax.X < position.X)
				{
					this.boundingBoxMax.X = position.X;
				}
				if (this.boundingBoxMax.Y < position.Y)
				{
					this.boundingBoxMax.Y = position.Y;
				}
				if (this.boundingBoxMax.Z < position.Z)
				{
					this.boundingBoxMax.Z = position.Z;
				}
				if (this.boundingBoxMin.X > position.X)
				{
					this.boundingBoxMin.X = position.X;
				}
				if (this.boundingBoxMin.Y > position.Y)
				{
					this.boundingBoxMin.Y = position.Y;
				}
				if (this.boundingBoxMin.Z > position.Z)
				{
					this.boundingBoxMin.Z = position.Z;
				}
			}
			this.boundingSphereCenter = (this.boundingBoxMax + this.boundingBoxMin) * 0.5f;
			this.boundingSphereR = (this.boundingBoxMax - this.boundingBoxMin).Length * 0.5f;
		}

	


		public override int Count
		{
			get
			{
				IList<int> r;
				return (r = this.GetIndexReader(Streams.Position, 0)) == null ? 0 : r.Count;
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

		Dictionary<StreamKey,IList<int>> streams = new Dictionary<StreamKey, IList<int>>();

		public T SetIndexStream<T>(string key, int channel, T stream) where T:IList<int>
		{
			 streams[new StreamKey(key, channel)] = stream;
			return stream;
		}

		public override IList<int> GetIndexReader(string key, int channel)
		{
			IList<int> streamReader;
			return !this.streams.TryGetValue(new StreamKey(key, channel), out streamReader) ? null : streamReader;
		}

		public IList<int> GetIndexStream(string key, int channel)
		{
			IList<int> streamReader;
			return !this.streams.TryGetValue(new StreamKey(key, channel), out streamReader) ? null : streamReader;
			
		}
	}
}