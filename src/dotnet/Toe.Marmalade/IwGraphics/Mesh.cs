using System.Collections;
using System.Collections.Generic;
using System.Drawing;

using OpenTK;

using Toe.Resources;
using Toe.Utils.Mesh;

namespace Toe.Marmalade.IwGraphics
{
	public class Mesh : Managed, IVertexSource
	{
		public static readonly uint TypeHash = Hash.Get("CMesh");

		private IResourceManager resourceManager;

		private string baseName;

		private float scale;

		private readonly MeshStream<Vector3> vertices = new MeshStream<Vector3>();

		private readonly MeshStream<Vector3> normals = new MeshStream<Vector3>();
		private readonly MeshStream<Vector3> binormals = new MeshStream<Vector3>();
		private readonly MeshStream<Vector3> tangents = new MeshStream<Vector3>();

		private readonly MeshStream<Vector2> uv0 = new MeshStream<Vector2>();

		private readonly MeshStream<Vector2> uv1 = new MeshStream<Vector2>();

		private MeshStream<Color> colors = new MeshStream<Color>();

		private IList<Surface> surfaces = new List<Surface>();

		public Mesh(IResourceManager resourceManager)
		{
			this.resourceManager = resourceManager;
		}

		public override uint ClassHashCode
		{
			get
			{
				return TypeHash;
			}
		}

		public string BaseName
		{
			get
			{
				return baseName;
			}
			set
			{
				if (baseName != value)
				{
					baseName = value;
					this.RaisePropertyChanged("BaseName");
				}
			}
		}

		public string useGeo
		{
			get
			{
				return useGeo;
			}
			set
			{
				if (useGeo != value)
				{
					useGeo = value;
					this.RaisePropertyChanged("useGeo");
				}
			}
		}

		public string useGroup
		{
			get
			{
				return useGroup;
			}
			set
			{
				if (useGeo != value)
				{
					useGroup = value;
					this.RaisePropertyChanged("useGroup");
				}
			}
		}

		public float Scale
		{
			get
			{
				return scale;
			}
			set
			{
				if (scale != value)
				{
					scale = value;
					this.RaisePropertyChanged("Scale");
				}
			}
		}

		public MeshStream<Vector3> Vertices
		{
			get
			{
				return vertices;
			}
		}

		public MeshStream<Vector3> Normals
		{
			get
			{
				return normals;
			}
		}

		public MeshStream<Color> Colors
		{
			get
			{
				return colors;
			}
			
		}
		public MeshStream<Vector2> UV0
		{
			get
			{
				return uv0;
			}

		}
		public MeshStream<Vector2> UV1
		{
			get
			{
				return uv1;
			}

		}
		public IList<Surface> Surfaces
		{
			get
			{
				return surfaces;
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
		public IEnumerator<Vertex> GetEnumerator()
		{
			foreach (var surface in surfaces)
			{
				foreach (var v in surface)
				{
					yield return v;
				}
			}
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
				return vertices != null && vertices.Count > 0;
			}
		}

		public bool IsNormalStreamAvailable
		{
			get
			{
				return normals != null && normals.Count > 0;
			}
		}

		public bool IsBinormalStreamAvailable
		{
			get
			{
				return binormals != null && binormals.Count > 0;
			}
		}

		public bool IsTangentStreamAvailable
		{
			get
			{
				return tangents != null && tangents.Count > 0;
			}
		}

		public bool IsColorStreamAvailable
		{
			get
			{
				return colors != null && colors.Count > 0;
			}
		}

		public bool IsUV0StreamAvailable
		{
			get
			{
				return uv0 != null && uv0.Count > 0;
			}
		}

		public bool IsUV1StreamAvailable
		{
			get
			{
				return uv1 != null && uv1.Count > 0;
			}
		}

		public VertexSourceType VertexSourceType
		{
			get
			{
				if (surfaces.Count > 0) return surfaces[0].VertexSourceType;
				return VertexSourceType.TrianleList;
			}
		}

		#endregion
	}
}