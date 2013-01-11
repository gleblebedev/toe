
using System.Collections;
using System.Collections.Generic;

namespace Toe.Utils.Mesh
{
	public abstract class BaseSubmesh : ISubMesh
	{
		private IMaterial material;

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

		private string name;

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

		#region Implementation of IEnumerable

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public abstract IEnumerator<Vertex> GetEnumerator();

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

		#region Implementation of IVertexSource

		public abstract bool IsVertexStreamAvailable { get; }

		public abstract bool IsNormalStreamAvailable { get; }

		public abstract bool IsBinormalStreamAvailable { get; }

		public abstract bool IsTangentStreamAvailable { get; }

		public abstract bool IsColorStreamAvailable { get; }

		public abstract bool IsUV0StreamAvailable { get; }

		public abstract bool IsUV1StreamAvailable { get; }

		VertexSourceType vertexSourceType = VertexSourceType.TrianleList;

		public virtual VertexSourceType VertexSourceType
		{
			get
			{
				return vertexSourceType;
			}
			set
			{
				vertexSourceType = value;
			}
		}


		#endregion
	}
}