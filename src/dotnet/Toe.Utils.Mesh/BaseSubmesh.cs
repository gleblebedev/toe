using System.Collections;
using System.Collections.Generic;

using OpenTK;

namespace Toe.Utils.Mesh
{
	public abstract class BaseSubmesh : ISubMesh
	{
		#region Constants and Fields

		protected Vector3 boundingBoxMax;

		protected Vector3 boundingBoxMin;

		protected Vector3 boundingSphereCenter;

		protected float boundingSphereR;

		private bool areBoundsValid;

		private IMaterial material;

		private string name;

		private VertexSourceType vertexSourceType = VertexSourceType.TrianleList;

		#endregion

		#region Public Properties

		public Vector3 BoundingBoxMax
		{
			get
			{
				this.CalculateBounds();
				return this.boundingBoxMax;
			}
		}

		public Vector3 BoundingBoxMin
		{
			get
			{
				this.CalculateBounds();
				return this.boundingBoxMin;
			}
		}

		public Vector3 BoundingSphereCenter
		{
			get
			{
				this.CalculateBounds();
				return this.boundingSphereCenter;
			}
		}

		public float BoundingSphereR
		{
			get
			{
				this.CalculateBounds();
				return this.boundingSphereR;
			}
		}

		public abstract int Count { get; }

		public IMaterial Material
		{
			get
			{
				return this.material;
			}
			set
			{
				if (this.material != value)
				{
					this.material = value;
				}
			}
		}

		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (this.name != value)
				{
					this.name = value;
				}
			}
		}

		public object RenderData { get; set; }

		public virtual VertexSourceType VertexSourceType
		{
			get
			{
				return this.vertexSourceType;
			}
			set
			{
				this.vertexSourceType = value;
			}
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public abstract IEnumerator<int> GetEnumerator();

		public void InvalidateBounds()
		{
			this.areBoundsValid = false;
		}

		#endregion

		#region Explicit Interface Methods

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion

		#region Methods

		protected abstract void CalculateActualBounds();

		protected void CalculateBounds()
		{
			if (this.areBoundsValid)
			{
				return;
			}
			this.areBoundsValid = true;
			this.CalculateActualBounds();
		}

		#endregion
	}
}