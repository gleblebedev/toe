using System.Collections.Generic;

namespace Toe.Utils.Mesh
{
	public class VertexBufferSubmesh : BaseSubmesh, ISubMesh
	{
		#region Constants and Fields

		private readonly List<int> indices = new List<int>();

		private readonly VertexBufferMesh mesh;

		#endregion

		#region Constructors and Destructors

		public VertexBufferSubmesh(VertexBufferMesh mesh)
		{
			this.mesh = mesh;
			this.VertexSourceType = VertexSourceType.TrianleList;
		}

		#endregion

		#region Public Properties

		public override int Count
		{
			get
			{
				return this.indices.Count;
			}
		}

		#endregion

		#region Public Methods and Operators

		public void Add(int i)
		{
			this.indices.Add(i);
		}

		public void Add(ref Vertex v)
		{
			this.Add(this.mesh.VertexBuffer.Add(v));
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public override IEnumerator<int> GetEnumerator()
		{
			foreach (var i in this.indices)
			{
				yield return i;
			}
		}

		#endregion

		#region Methods

		protected override void CalculateActualBounds()
		{
			foreach (var index in this.indices)
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
			this.boundingSphereCenter = (this.boundingBoxMax + this.boundingBoxMin) * 0.5f;
			this.boundingSphereR = (this.boundingBoxMax - this.boundingBoxMin).Length * 0.5f;
		}

		#endregion
	}
}