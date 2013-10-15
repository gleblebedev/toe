using System.Collections.Generic;

using Toe.Utils.ToeMath;

namespace Toe.Utils.Mesh
{
	public class VertexBufferSubmesh<TVertex> : BaseSubmesh, ISubMesh
	{
		#region Constants and Fields

		private readonly List<int> indices = new List<int>();

		private readonly VertexBufferMesh<TVertex> mesh;

		#endregion

		#region Constructors and Destructors

		public VertexBufferSubmesh(VertexBufferMesh<TVertex> mesh)
		{
			this.mesh = mesh;
			this.VertexSourceType = VertexSourceType.TriangleList;
		}

		#endregion

		#region Public Properties


		#endregion

		#region Public Methods and Operators

		public void Add(int i)
		{
			this.indices.Add(i);
		}

		public void Add(ref TVertex v)
		{
			this.mesh.VertexBuffer.Add(v);
		}

	
		
		#endregion

		#region Methods

		/// <summary>
		/// Get number of indices.
		/// Each stream should have same number of indices.
		/// </summary>
		public override int Count
		{
			get
			{
				throw new System.NotImplementedException();
			}
		}

		public override IList<int> GetIndexReader(string key, int channel)
		{
			throw new System.NotImplementedException();
		}

		protected override void CalculateActualBounds()
		{
			var positions = this.mesh.GetStreamReader<Float3>(Streams.Position, 0);
			foreach (var index in this.indices)
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

		#endregion
	}
}