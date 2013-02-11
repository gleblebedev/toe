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
		public override IEnumerator<int> GetEnumerator()
		{
			foreach (var i in indices)
			{
				yield return i;
			}
		}

		protected override void CalculateActualBounds()
		{
			foreach (var index in indices)
			{
				var position = this.mesh.VertexBuffer[index].Position;
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
			boundingSphereCenter = (boundingBoxMax + boundingBoxMin) * 0.5f;
			boundingSphereR = (boundingBoxMax - boundingBoxMin).Length * 0.5f;
		}

		public override int Count
		{
			get
			{
				return indices.Count;
			}
		}


		#endregion
	}
}