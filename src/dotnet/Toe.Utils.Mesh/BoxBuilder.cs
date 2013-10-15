using System.Drawing;

using Toe.Utils.ToeMath;

namespace Toe.Utils.Mesh
{
	public static class BoxBuilder
	{
		#region Public Methods and Operators

		public static IMesh BuildSoftEdgedBox(float size)
		{
			return BuildSoftEdgedBox(size, size, size);
		}

		public static IMesh BuildSoftEdgedBox(float x, float y, float z)
		{
			Float3[] p = new[]
				{
					new Float3(-x / 2.0f, -y / 2.0f, z / 2.0f), new Float3(x / 2.0f, -y / 2.0f, z / 2.0f),
					new Float3(-x / 2.0f, y / 2.0f, z / 2.0f), new Float3(x / 2.0f, y / 2.0f, z / 2.0f),
					new Float3(-x / 2.0f, y / 2.0f, -z / 2.0f), new Float3(x / 2.0f, y / 2.0f, -z / 2.0f),
					new Float3(-x / 2.0f, -y / 2.0f, -z / 2.0f), new Float3(x / 2.0f, -y / 2.0f, -z / 2.0f),
				};
			Float3[] n = new[]
				{
					new Float3(-0.57735f, -0.57735f, -0.57735f), new Float3(-0.57735f, 0.57735f, -0.57735f),
					new Float3(0.57735f, -0.57735f, -0.57735f), new Float3(0.57735f, 0.57735f, -0.57735f),
					new Float3(-0.57735f, -0.57735f, 0.57735f), new Float3(-0.57735f, 0.57735f, 0.57735f),
					new Float3(0.57735f, -0.57735f, 0.57735f), new Float3(0.57735f, 0.57735f, 0.57735f),
				};
			Float3[] uv = new[]
				{
					new Float3(0.0f, 0.0f, 0.0f), new Float3(1.0f, 0.0f, 0.0f), new Float3(1.0f, 1.0f, 0.0f),
					new Float3(0.0f, 1.0f, 0.0f),
				};

			var res = new SeparateStreamsMesh();
			res.SetStream(Streams.Position, 0, new ArrayMeshStream<Float3>(p));
			res.SetStream(Streams.Normal, 0, new ArrayMeshStream<Float3>(n));
			res.SetStream(Streams.TexCoord, 0, new ArrayMeshStream<Float3>(uv));
			res.SetStream(Streams.TexCoord, 1, new ArrayMeshStream<Float3>(uv));
			res.SetStream(Streams.Color, 0, new ArrayMeshStream<Color>(new Color[] { Color.FromArgb(255, 255, 255, 255) }));

			var t2 = Float3.Normalize(p[7] - p[6]);
			var b2 = Float3.Cross(Float3.Normalize(n[0] + n[2] + n[3] + n[1]), t2);
			var t3 = Float3.Normalize(p[1] - p[0]);
			var b3 = Float3.Cross(Float3.Normalize(n[4] + n[6] + n[2] + n[0]), t3);
			var t4 = Float3.Normalize(p[5] - p[3]);
			var b4 = Float3.Cross(Float3.Normalize(n[7] + n[3] + n[2] + n[6]), t4);
			var t5 = Float3.Normalize(p[2] - p[4]);
			var b5 = Float3.Cross(Float3.Normalize(n[1] + n[5] + n[4] + n[0]), t5);
			res.SetStream(Streams.Tangent, 0, new ArrayMeshStream<Float3>(new Float3[] { new Float3(1, 0, 0), new Float3(1, 0, 0), t2,t3,t4,t5 }));
			res.SetStream(Streams.Binormal, 0, new ArrayMeshStream<Float3>(new Float3[] { new Float3(0, 1, 0), new Float3(0, 0, -1), b2,b3,b4,b5 }));

			var s = res.CreateSubmesh();
			s.VertexSourceType = VertexSourceType.QuadList;

			var positionIndices = s.SetIndexStream(Streams.Position, 0, new ListMeshStream<int>(24));
			var normalIndices = s.SetIndexStream(Streams.Normal, 0, new ListMeshStream<int>(24));
			var uv0Indices = s.SetIndexStream(Streams.TexCoord, 0, new ListMeshStream<int>(24));
			var uv1Indices = s.SetIndexStream(Streams.TexCoord, 1, new ListMeshStream<int>(24));
			var colorIndices = s.SetIndexStream(Streams.Color, 0, new ListMeshStream<int>(24));
			var tangentIndices = s.SetIndexStream(Streams.Tangent, 0, new ListMeshStream<int>(24));
			var binormalIndices = s.SetIndexStream(Streams.Binormal, 0, new ListMeshStream<int>(24));

			Vertex vertex;

			Float3 t;
			Float3 b;

			// Quad 0 TOP
			//t = new Float3(1, 0, 0);
			//b = new Float3(0, 1, 0); //Float3.Cross(new Float3(0,0,1),t)

			positionIndices.Add(0); normalIndices.Add(4); colorIndices.Add(0); uv0Indices.Add(3); uv1Indices.Add(3); tangentIndices.Add(0); binormalIndices.Add(0);
			positionIndices.Add(1); normalIndices.Add(6); colorIndices.Add(0); uv0Indices.Add(2); uv1Indices.Add(2); tangentIndices.Add(0); binormalIndices.Add(0);
			positionIndices.Add(3); normalIndices.Add(7); colorIndices.Add(0); uv0Indices.Add(1); uv1Indices.Add(1); tangentIndices.Add(0); binormalIndices.Add(0);
			positionIndices.Add(2); normalIndices.Add(5); colorIndices.Add(0); uv0Indices.Add(0); uv1Indices.Add(0); tangentIndices.Add(0); binormalIndices.Add(0);

			//vertex = new Vertex
			//	{ Position = p[0], Color = color, Normal = n[4], UV0 = uv[3], UV1 = uv[3], Tangent = t, Binormal = b };
			//s.Add(ref vertex);
			//vertex = new Vertex
			//	{ Position = p[1], Color = color, Normal = n[6], UV0 = uv[2], UV1 = uv[2], Tangent = t, Binormal = b };
			//s.Add(ref vertex);
			//vertex = new Vertex
			//	{ Position = p[3], Color = color, Normal = n[7], UV0 = uv[1], UV1 = uv[1], Tangent = t, Binormal = b };
			//s.Add(ref vertex);
			//vertex = new Vertex
			//	{ Position = p[2], Color = color, Normal = n[5], UV0 = uv[0], UV1 = uv[0], Tangent = t, Binormal = b };
			//s.Add(ref vertex);

			positionIndices.Add(2); normalIndices.Add(5); colorIndices.Add(0); uv0Indices.Add(3); uv1Indices.Add(3); tangentIndices.Add(1); binormalIndices.Add(1);
			positionIndices.Add(3); normalIndices.Add(7); colorIndices.Add(0); uv0Indices.Add(2); uv1Indices.Add(2); tangentIndices.Add(1); binormalIndices.Add(1);
			positionIndices.Add(5); normalIndices.Add(3); colorIndices.Add(0); uv0Indices.Add(1); uv1Indices.Add(1); tangentIndices.Add(1); binormalIndices.Add(1);
			positionIndices.Add(4); normalIndices.Add(1); colorIndices.Add(0); uv0Indices.Add(0); uv1Indices.Add(0); tangentIndices.Add(1); binormalIndices.Add(1);

			//// Quad 1
			//t = new Float3(1, 0, 0);
			//b = new Float3(0, 0, -1);
			//vertex = new Vertex
			//	{ Position = p[2], Color = color, Normal = n[5], UV0 = uv[3], UV1 = uv[3], Tangent = t, Binormal = b };
			//s.Add(ref vertex);
			//vertex = new Vertex
			//	{ Position = p[3], Color = color, Normal = n[7], UV0 = uv[2], UV1 = uv[2], Tangent = t, Binormal = b };
			//s.Add(ref vertex);
			//vertex = new Vertex
			//	{ Position = p[5], Color = color, Normal = n[3], UV0 = uv[1], UV1 = uv[1], Tangent = t, Binormal = b };
			//s.Add(ref vertex);
			//vertex = new Vertex
			//	{ Position = p[4], Color = color, Normal = n[1], UV0 = uv[0], UV1 = uv[0], Tangent = t, Binormal = b };
			//s.Add(ref vertex);

			positionIndices.Add(4); normalIndices.Add(1); colorIndices.Add(0); uv0Indices.Add(3); uv1Indices.Add(3); tangentIndices.Add(2); binormalIndices.Add(2);
			positionIndices.Add(5); normalIndices.Add(3); colorIndices.Add(0); uv0Indices.Add(2); uv1Indices.Add(2); tangentIndices.Add(2); binormalIndices.Add(2);
			positionIndices.Add(7); normalIndices.Add(2); colorIndices.Add(0); uv0Indices.Add(1); uv1Indices.Add(1); tangentIndices.Add(2); binormalIndices.Add(2);
			positionIndices.Add(6); normalIndices.Add(0); colorIndices.Add(0); uv0Indices.Add(0); uv1Indices.Add(0); tangentIndices.Add(2); binormalIndices.Add(2);

			//// Quad 2
			//t = Float3.Normalize(p[7] - p[6]);
			//b = Float3.Cross(Float3.Normalize(n[0] + n[2] + n[3] + n[1]), t);
			//vertex = new Vertex
			//	{ Position = p[4], Color = color, Normal = n[1], UV0 = uv[3], UV1 = uv[3], Tangent = t, Binormal = b };
			//s.Add(ref vertex);
			//vertex = new Vertex
			//	{ Position = p[5], Color = color, Normal = n[3], UV0 = uv[2], UV1 = uv[2], Tangent = t, Binormal = b };
			//s.Add(ref vertex);
			//vertex = new Vertex
			//	{ Position = p[7], Color = color, Normal = n[2], UV0 = uv[1], UV1 = uv[1], Tangent = t, Binormal = b };
			//s.Add(ref vertex);
			//vertex = new Vertex
			//	{ Position = p[6], Color = color, Normal = n[0], UV0 = uv[0], UV1 = uv[0], Tangent = t, Binormal = b };
			//s.Add(ref vertex);

			positionIndices.Add(6); normalIndices.Add(0); colorIndices.Add(0); uv0Indices.Add(3); uv1Indices.Add(3); tangentIndices.Add(3); binormalIndices.Add(3);
			positionIndices.Add(7); normalIndices.Add(2); colorIndices.Add(0); uv0Indices.Add(2); uv1Indices.Add(2); tangentIndices.Add(3); binormalIndices.Add(3);
			positionIndices.Add(1); normalIndices.Add(6); colorIndices.Add(0); uv0Indices.Add(1); uv1Indices.Add(1); tangentIndices.Add(3); binormalIndices.Add(3);
			positionIndices.Add(0); normalIndices.Add(4); colorIndices.Add(0); uv0Indices.Add(0); uv1Indices.Add(0); tangentIndices.Add(3); binormalIndices.Add(3);

			//// Quad 3
			//t = Float3.Normalize(p[1] - p[0]);
			//b = Float3.Cross(Float3.Normalize(n[4] + n[6] + n[2] + n[0]), t);
			//vertex = new Vertex
			//	{ Position = p[6], Color = color, Normal = n[0], UV0 = uv[3], UV1 = uv[3], Tangent = t, Binormal = b };
			//s.Add(ref vertex);
			//vertex = new Vertex
			//	{ Position = p[7], Color = color, Normal = n[2], UV0 = uv[2], UV1 = uv[2], Tangent = t, Binormal = b };
			//s.Add(ref vertex);
			//vertex = new Vertex
			//	{ Position = p[1], Color = color, Normal = n[6], UV0 = uv[1], UV1 = uv[1], Tangent = t, Binormal = b };
			//s.Add(ref vertex);
			//vertex = new Vertex
			//	{ Position = p[0], Color = color, Normal = n[4], UV0 = uv[0], UV1 = uv[0], Tangent = t, Binormal = b };
			//s.Add(ref vertex);

			positionIndices.Add(1); normalIndices.Add(6); colorIndices.Add(0); uv0Indices.Add(3); uv1Indices.Add(3); tangentIndices.Add(4); binormalIndices.Add(4);
			positionIndices.Add(7); normalIndices.Add(2); colorIndices.Add(0); uv0Indices.Add(2); uv1Indices.Add(2); tangentIndices.Add(4); binormalIndices.Add(4);
			positionIndices.Add(5); normalIndices.Add(3); colorIndices.Add(0); uv0Indices.Add(1); uv1Indices.Add(1); tangentIndices.Add(4); binormalIndices.Add(4);
			positionIndices.Add(3); normalIndices.Add(7); colorIndices.Add(0); uv0Indices.Add(0); uv1Indices.Add(0); tangentIndices.Add(4); binormalIndices.Add(4);

			//// Quad 4
			//t = Float3.Normalize(p[5] - p[3]);
			//b = Float3.Cross(Float3.Normalize(n[7] + n[3] + n[2] + n[6]), t);
			//vertex = new Vertex
			//	{ Position = p[1], Color = color, Normal = n[6], UV0 = uv[3], UV1 = uv[3], Tangent = t, Binormal = b };
			//s.Add(ref vertex);
			//vertex = new Vertex
			//	{ Position = p[7], Color = color, Normal = n[2], UV0 = uv[2], UV1 = uv[2], Tangent = t, Binormal = b };
			//s.Add(ref vertex);
			//vertex = new Vertex
			//	{ Position = p[5], Color = color, Normal = n[3], UV0 = uv[1], UV1 = uv[1], Tangent = t, Binormal = b };
			//s.Add(ref vertex);
			//vertex = new Vertex
			//	{ Position = p[3], Color = color, Normal = n[7], UV0 = uv[0], UV1 = uv[0], Tangent = t, Binormal = b };
			//s.Add(ref vertex);

			positionIndices.Add(6); normalIndices.Add(0); colorIndices.Add(0); uv0Indices.Add(3); uv1Indices.Add(3); tangentIndices.Add(5); binormalIndices.Add(5);
			positionIndices.Add(0); normalIndices.Add(4); colorIndices.Add(0); uv0Indices.Add(2); uv1Indices.Add(2); tangentIndices.Add(5); binormalIndices.Add(5);
			positionIndices.Add(2); normalIndices.Add(5); colorIndices.Add(0); uv0Indices.Add(1); uv1Indices.Add(1); tangentIndices.Add(5); binormalIndices.Add(5);
			positionIndices.Add(4); normalIndices.Add(1); colorIndices.Add(0); uv0Indices.Add(0); uv1Indices.Add(0); tangentIndices.Add(5); binormalIndices.Add(5);

			//// Quad 5
			//t = Float3.Normalize(p[2] - p[4]);
			//b = Float3.Cross(Float3.Normalize(n[1] + n[5] + n[4] + n[0]), t);
			//vertex = new Vertex
			//	{ Position = p[6], Color = color, Normal = n[0], UV0 = uv[3], UV1 = uv[3], Tangent = t, Binormal = b };
			//s.Add(ref vertex);
			//vertex = new Vertex
			//	{ Position = p[0], Color = color, Normal = n[4], UV0 = uv[2], UV1 = uv[2], Tangent = t, Binormal = b };
			//s.Add(ref vertex);
			//vertex = new Vertex
			//	{ Position = p[2], Color = color, Normal = n[5], UV0 = uv[1], UV1 = uv[1], Tangent = t, Binormal = b };
			//s.Add(ref vertex);
			//vertex = new Vertex
			//	{ Position = p[4], Color = color, Normal = n[1], UV0 = uv[0], UV1 = uv[0], Tangent = t, Binormal = b };
			//s.Add(ref vertex);

			return res;
		}

		#endregion
	}
}