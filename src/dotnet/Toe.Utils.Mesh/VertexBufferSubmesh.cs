using System.Collections.Generic;

using OpenTK.Graphics.OpenGL;

namespace Toe.Utils.Mesh
{
	public class VertexBufferSubmesh : BaseSubmesh,ISubMesh
	{
		readonly List<int> indices = new List<int>();

		private readonly VertexBufferMesh mesh;

		public VertexBufferSubmesh(VertexBufferMesh mesh):base()
		{
			this.mesh = mesh;
			this.VertexSourceType = VertexSourceType.TrianleList;
		}

		public void Add(int i)
		{
			indices.Add(i);
		}
		public void Add(ref Vertex v)
		{
			this.Add(this.mesh.VertexBuffer.Add(v));
		}

		#region Overrides of BaseSubmesh

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public override IEnumerator<Vertex> GetEnumerator()
		{
			foreach (var i in indices)
			{
				yield return this.mesh.VertexBuffer[i];
			}
		}

		public override bool IsVertexStreamAvailable
		{
			get
			{
				return mesh.IsVertexStreamAvailable;
			}
		}

		public override bool IsNormalStreamAvailable
		{
			get
			{
				return mesh.IsNormalStreamAvailable;
			}
		}

		public override bool IsBinormalStreamAvailable
		{
			get
			{
				return mesh.IsBinormalStreamAvailable;
			}
		}

		public override bool IsTangentStreamAvailable
		{
			get
			{
				return mesh.IsTangentStreamAvailable;
			}
		}

		public override bool IsColorStreamAvailable
		{
			get
			{
				return mesh.IsColorStreamAvailable;
			}
		}

		public override bool IsUV0StreamAvailable
		{
			get
			{
				return mesh.IsUV0StreamAvailable;
			}
		}

		public override bool IsUV1StreamAvailable
		{
			get
			{
				return mesh.IsUV1StreamAvailable;
			}
		}

		#endregion
	}
}