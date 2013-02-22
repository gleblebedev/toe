using System.Drawing;

using OpenTK;

namespace Toe.Utils.Mesh
{
	public static class BoxBuilder
	{
		public static IMesh BuildSoftEdgedBox(float size)
		{
			return BuildSoftEdgedBox(size, size, size);
		}
		public static IMesh BuildSoftEdgedBox(float x, float y, float z)
		{
				Vector3[] p = new[]
				{
					new Vector3(-x/2.0f, -y/2.0f, z/2.0f), new Vector3(x/2.0f, -y/2.0f, z/2.0f), new Vector3(-x/2.0f, y/2.0f, z/2.0f),
					new Vector3(x/2.0f, y/2.0f, z/2.0f), new Vector3(-x/2.0f, y/2.0f, -z/2.0f), new Vector3(x/2.0f, y/2.0f, -z/2.0f),
					new Vector3(-x/2.0f, -y/2.0f, -z/2.0f), new Vector3(x/2.0f, -y/2.0f, -z/2.0f),
				};
			Vector3[] n = new[]
				{
					new Vector3(-0.57735f, -0.57735f, -0.57735f), new Vector3(-0.57735f, 0.57735f, -0.57735f),
					new Vector3(0.57735f, -0.57735f, -0.57735f), new Vector3(0.57735f, 0.57735f, -0.57735f),
					new Vector3(-0.57735f, -0.57735f, 0.57735f), new Vector3(-0.57735f, 0.57735f, 0.57735f),
					new Vector3(0.57735f, -0.57735f, 0.57735f), new Vector3(0.57735f, 0.57735f, 0.57735f),
				};
			Vector3[] uv = new[] { new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f), };

			var res = new VertexBufferMesh();
			var s = res.CreateSubmesh() as VertexBufferSubmesh;
			s.VertexSourceType = VertexSourceType.QuadList;
			Vertex vertex;

			Color color = Color.FromArgb(255, 255, 255, 255);

			Vector3 t;
			Vector3 b;

			// Quad 0 TOP
			t = new Vector3(1, 0, 0);
			b = new Vector3(0, 1, 0); //Vector3.Cross(new Vector3(0,0,1),t)
			vertex = new Vertex { Position = p[0], Color = color, Normal = n[4], UV0 = uv[3], UV1 = uv[3], Tangent = t, Binormal = b };
			s.Add(ref vertex);
			vertex = new Vertex { Position = p[1], Color = color, Normal = n[6], UV0 = uv[2], UV1 = uv[2], Tangent = t, Binormal = b };
			s.Add(ref vertex);
			vertex = new Vertex { Position = p[3], Color = color, Normal = n[7], UV0 = uv[1], UV1 = uv[1], Tangent = t, Binormal = b };
			s.Add(ref vertex);
			vertex = new Vertex { Position = p[2], Color = color, Normal = n[5], UV0 = uv[0], UV1 = uv[0], Tangent = t, Binormal = b };
			s.Add(ref vertex);

			// Quad 1
			t = new Vector3(1, 0, 0);
			b = new Vector3(0, 0, -1);
			vertex = new Vertex { Position = p[2], Color = color, Normal = n[5], UV0 = uv[3], UV1 = uv[3], Tangent = t, Binormal = b };
			s.Add(ref vertex);
			vertex = new Vertex { Position = p[3], Color = color, Normal = n[7], UV0 = uv[2], UV1 = uv[2], Tangent = t, Binormal = b };
			s.Add(ref vertex);
			vertex = new Vertex { Position = p[5], Color = color, Normal = n[3], UV0 = uv[1], UV1 = uv[1], Tangent = t, Binormal = b };
			s.Add(ref vertex);
			vertex = new Vertex { Position = p[4], Color = color, Normal = n[1], UV0 = uv[0], UV1 = uv[0], Tangent = t, Binormal = b };
			s.Add(ref vertex);

			// Quad 2
			t = Vector3.Normalize(p[7]-p[6]);
			b = Vector3.Cross(Vector3.Normalize(n[0] + n[2] + n[3] + n[1]), t);
			vertex = new Vertex { Position = p[4], Color = color, Normal = n[1], UV0 = uv[3], UV1 = uv[3], Tangent = t, Binormal = b };
			s.Add(ref vertex);
			vertex = new Vertex { Position = p[5], Color = color, Normal = n[3], UV0 = uv[2], UV1 = uv[2], Tangent = t, Binormal = b };
			s.Add(ref vertex);
			vertex = new Vertex { Position = p[7], Color = color, Normal = n[2], UV0 = uv[1], UV1 = uv[1], Tangent = t, Binormal = b };
			s.Add(ref vertex);
			vertex = new Vertex { Position = p[6], Color = color, Normal = n[0], UV0 = uv[0], UV1 = uv[0], Tangent = t, Binormal = b };
			s.Add(ref vertex);

			// Quad 3
			t = Vector3.Normalize(p[1] - p[0]);
			b = Vector3.Cross(Vector3.Normalize(n[4] + n[6] + n[2] + n[0]), t);
			vertex = new Vertex { Position = p[6], Color = color, Normal = n[0], UV0 = uv[3], UV1 = uv[3], Tangent = t, Binormal = b };
			s.Add(ref vertex);
			vertex = new Vertex { Position = p[7], Color = color, Normal = n[2], UV0 = uv[2], UV1 = uv[2], Tangent = t, Binormal = b };
			s.Add(ref vertex);
			vertex = new Vertex { Position = p[1], Color = color, Normal = n[6], UV0 = uv[1], UV1 = uv[1], Tangent = t, Binormal = b };
			s.Add(ref vertex);
			vertex = new Vertex { Position = p[0], Color = color, Normal = n[4], UV0 = uv[0], UV1 = uv[0], Tangent = t, Binormal = b };
			s.Add(ref vertex);

			// Quad 4
			t = Vector3.Normalize(p[5] - p[3]);
			b = Vector3.Cross(Vector3.Normalize(n[7] + n[3] + n[2] + n[6]), t);
			vertex = new Vertex { Position = p[1], Color = color, Normal = n[6], UV0 = uv[3], UV1 = uv[3], Tangent = t, Binormal = b };
			s.Add(ref vertex);
			vertex = new Vertex { Position = p[7], Color = color, Normal = n[2], UV0 = uv[2], UV1 = uv[2], Tangent = t, Binormal = b };
			s.Add(ref vertex);
			vertex = new Vertex { Position = p[5], Color = color, Normal = n[3], UV0 = uv[1], UV1 = uv[1], Tangent = t, Binormal = b };
			s.Add(ref vertex);
			vertex = new Vertex { Position = p[3], Color = color, Normal = n[7], UV0 = uv[0], UV1 = uv[0], Tangent = t, Binormal = b };
			s.Add(ref vertex);

			// Quad 5
			t = Vector3.Normalize(p[2] - p[4]);
			b = Vector3.Cross(Vector3.Normalize(n[1] + n[5] + n[4] + n[0]), t);
			vertex = new Vertex { Position = p[6], Color = color, Normal = n[0], UV0 = uv[3], UV1 = uv[3], Tangent = t, Binormal = b };
			s.Add(ref vertex);
			vertex = new Vertex { Position = p[0], Color = color, Normal = n[4], UV0 = uv[2], UV1 = uv[2], Tangent = t, Binormal = b };
			s.Add(ref vertex);
			vertex = new Vertex { Position = p[2], Color = color, Normal = n[5], UV0 = uv[1], UV1 = uv[1], Tangent = t, Binormal = b };
			s.Add(ref vertex);
			vertex = new Vertex { Position = p[4], Color = color, Normal = n[1], UV0 = uv[0], UV1 = uv[0], Tangent = t, Binormal = b };
			s.Add(ref vertex);

			return res;
		}

	}
}