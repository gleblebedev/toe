using System.Collections;
using System.Collections.Generic;

using Toe.Resources;
using Toe.Utils.Mesh;

namespace Toe.Marmalade.IwGraphics
{
	public class Surface : Managed, IVertexSource
	{
		public static readonly uint TypeHash = Hash.Get("CSurface");

		private ResourceReference material;

		public Surface(IResourceManager resourceManager)
		{
			this.material = new ResourceReference(Toe.Marmalade.IwGx.Material.TypeHash, resourceManager, this);
		}

		private Mesh mesh;

		public Mesh Mesh
		{
			get
			{
				return this.mesh;
			}
			set
			{
				this.mesh = value;
			}
		}

		public override uint ClassHashCode
		{
			get
			{
				return TypeHash;
			}
		}

		public ResourceReference Material
		{
			get
			{
				return material;
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
		public virtual IEnumerator<Vertex> GetEnumerator()
		{
			throw new System.NotImplementedException();
		}

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

		public bool IsVertexStreamAvailable
		{
			get
			{
				return mesh.IsVertexStreamAvailable;
			}
		}

		public bool IsNormalStreamAvailable
		{
			get
			{
				return mesh.IsNormalStreamAvailable;
			}
		}

		public bool IsBinormalStreamAvailable
		{
			get
			{
				return mesh.IsBinormalStreamAvailable;
			}
		}

		public bool IsTangentStreamAvailable
		{
			get
			{
				return mesh.IsTangentStreamAvailable;
			}
		}

		public bool IsColorStreamAvailable
		{
			get
			{
				return mesh.IsColorStreamAvailable;
			}
		}

		public bool IsUV0StreamAvailable
		{
			get
			{
				return mesh.IsUV0StreamAvailable;
			}
		}

		public bool IsUV1StreamAvailable
		{
			get
			{
				return mesh.IsUV1StreamAvailable;
			}
		}

		public virtual VertexSourceType VertexSourceType
		{
			get
			{
				return VertexSourceType.TrianleList;
			}
		}

		#endregion
	}
}