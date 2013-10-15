using System.Collections;
using System.Collections.Generic;


using Toe.Utils.ToeMath;

namespace Toe.Utils.Mesh
{
	public abstract class BaseSubmesh : ISubMesh
	{
		#region Constants and Fields

		protected Float3 boundingBoxMax;

		protected Float3 boundingBoxMin;

		protected Float3 boundingSphereCenter;

		protected float boundingSphereR;

		private bool areBoundsValid;

		private IMaterial material;

		private string name;

		private VertexSourceType vertexSourceType = VertexSourceType.TriangleList;

		#endregion

		#region Public Properties

		public Float3 BoundingBoxMax
		{
			get
			{
				this.CalculateBounds();
				return this.boundingBoxMax;
			}
		}

		public Float3 BoundingBoxMin
		{
			get
			{
				this.CalculateBounds();
				return this.boundingBoxMin;
			}
		}

		public Float3 BoundingSphereCenter
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

		public abstract IList<int> GetIndexReader(string key, int channel);

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



		public void InvalidateBounds()
		{
			this.areBoundsValid = false;
		}

		/// <summary>
		/// Get number of indices.
		/// Each stream should have same number of indices.
		/// </summary>
		public abstract int Count { get;  }

		#endregion

		#region Explicit Interface Methods


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