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

			vertex = new Vertex { Position = p[2], Color = color, Normal = n[5], UV0 = uv[0], UV1 = uv[0] };
			s.Add(ref vertex);
			vertex = new Vertex { Position = p[3], Color = color, Normal = n[7], UV0 = uv[1], UV1 = uv[1] };
			s.Add(ref vertex);
			vertex = new Vertex { Position = p[1], Color = color, Normal = n[6], UV0 = uv[2], UV1 = uv[2] };
			s.Add(ref vertex);
			vertex = new Vertex { Position = p[0], Color = color, Normal = n[4], UV0 = uv[3], UV1 = uv[3] };
			s.Add(ref vertex);

			//vertex = new Vertex { Position = p[2], Color = color, Normal = n[5], UV0 = uv[0], UV1 = uv[0] };
			//s.Add(ref vertex);
			//vertex = new Vertex { Position = p[3], Color = color, Normal = n[7], UV0 = uv[1], UV1 = uv[1] };
			//s.Add(ref vertex);
			//vertex = new Vertex { Position = p[1], Color = color, Normal = n[6], UV0 = uv[2], UV1 = uv[2] };
			//s.Add(ref vertex);
			//vertex = new Vertex { Position = p[0], Color = color, Normal = n[4], UV0 = uv[3], UV1 = uv[3] };
			//s.Add(ref vertex);

			//vertex = new Vertex { Position = p[4], Color = color, Normal = n[1], UV0 = uv[0], UV1 = uv[0] };
			//s.Add(ref vertex);
			//vertex = new Vertex { Position = p[5], Color = color, Normal = n[3], UV0 = uv[1], UV1 = uv[1] };
			//s.Add(ref vertex);
			//vertex = new Vertex { Position = p[3], Color = color, Normal = n[7], UV0 = uv[2], UV1 = uv[2] };
			//s.Add(ref vertex);
			//vertex = new Vertex { Position = p[2], Color = color, Normal = n[5], UV0 = uv[3], UV1 = uv[3] };
			//s.Add(ref vertex);

			//vertex = new Vertex { Position = p[6], Color = color, Normal = n[0], UV0 = uv[0], UV1 = uv[0] };
			//s.Add(ref vertex);
			//vertex = new Vertex { Position = p[7], Color = color, Normal = n[2], UV0 = uv[1], UV1 = uv[1] };
			//s.Add(ref vertex);
			//vertex = new Vertex { Position = p[5], Color = color, Normal = n[3], UV0 = uv[2], UV1 = uv[2] };
			//s.Add(ref vertex);
			//vertex = new Vertex { Position = p[4], Color = color, Normal = n[1], UV0 = uv[3], UV1 = uv[3] };
			//s.Add(ref vertex);

			//vertex = new Vertex { Position = p[0], Color = color, Normal = n[4], UV0 = uv[0], UV1 = uv[0] };
			//s.Add(ref vertex);
			//vertex = new Vertex { Position = p[1], Color = color, Normal = n[6], UV0 = uv[1], UV1 = uv[1] };
			//s.Add(ref vertex);
			//vertex = new Vertex { Position = p[7], Color = color, Normal = n[2], UV0 = uv[2], UV1 = uv[2] };
			//s.Add(ref vertex);
			//vertex = new Vertex { Position = p[6], Color = color, Normal = n[0], UV0 = uv[3], UV1 = uv[3] };
			//s.Add(ref vertex);

			//vertex = new Vertex { Position = p[3], Color = color, Normal = n[7], UV0 = uv[0], UV1 = uv[0] };
			//s.Add(ref vertex);
			//vertex = new Vertex { Position = p[5], Color = color, Normal = n[3], UV0 = uv[1], UV1 = uv[1] };
			//s.Add(ref vertex);
			//vertex = new Vertex { Position = p[7], Color = color, Normal = n[2], UV0 = uv[2], UV1 = uv[2] };
			//s.Add(ref vertex);
			//vertex = new Vertex { Position = p[1], Color = color, Normal = n[6], UV0 = uv[3], UV1 = uv[3] };
			//s.Add(ref vertex);

			//vertex = new Vertex { Position = p[4], Color = color, Normal = n[1], UV0 = uv[0], UV1 = uv[0] };
			//s.Add(ref vertex);
			//vertex = new Vertex { Position = p[2], Color = color, Normal = n[5], UV0 = uv[1], UV1 = uv[1] };
			//s.Add(ref vertex);
			//vertex = new Vertex { Position = p[0], Color = color, Normal = n[4], UV0 = uv[2], UV1 = uv[2] };
			//s.Add(ref vertex);
			//vertex = new Vertex { Position = p[6], Color = color, Normal = n[0], UV0 = uv[3], UV1 = uv[3] };
			//s.Add(ref vertex);

			return res;
		}

	}
}