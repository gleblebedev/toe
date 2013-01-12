using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using OpenTK;

using Toe.Resources;
using Toe.Utils.Mesh;

namespace Toe.Marmalade.IwGraphics
{
	public class Mesh : Managed, IVertexStreamSource
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


		#region Implementation of IVertexSource

		public int Count
		{
			get
			{
				return this.surfaces.Sum(submesh => submesh.Count);
			}
		}

		public bool IsVertexStreamAvailable
		{
			get
			{
				return vertices != null && vertices.Count > 0;
			}
		}

		public void VisitVertices(Vector3VisitorCallback callback)
		{
			foreach (var surface in surfaces)
			{
				var gl = surface as ModelBlockGLPrimBase;
				if (gl != null)
				{
					foreach (var i in gl.Indices)
					{
						var v = vertices[i];
						callback(ref v);
					}
				}
				else
				{
					var prim = surface as ModelBlockPrimBase;
					if (prim != null)
					{
						foreach (var i in prim.Indices)
						{
							var v = vertices[i.Vertex];
							callback(ref v);
						}
					}
				}
			}
		}

		public void VisitNormals(Vector3VisitorCallback callback)
		{
			foreach (var surface in surfaces)
			{
				var gl = surface as ModelBlockGLPrimBase;
				if (gl != null)
				{
					foreach (var i in gl.Indices)
					{
						var v = normals[i];
						callback(ref v);
					}
				}
				else
				{
					var prim = surface as ModelBlockPrimBase;
					if (prim != null)
					{
						foreach (var i in prim.Indices)
						{
							var v = normals[i.Normal];
							callback(ref v);
						}
					}
				}
			}
		}

		public void VisitColors(ColorVisitorCallback callback)
		{
			foreach (var surface in surfaces)
			{
				var gl = surface as ModelBlockGLPrimBase;
				if (gl != null)
				{
					foreach (var i in gl.Indices)
					{
						var v = colors[i];
						callback(ref v);
					}
				}
				else
				{
					var prim = surface as ModelBlockPrimBase;
					if (prim != null)
					{
						foreach (var i in prim.Indices)
						{
							var v = colors[i.Color];
							callback(ref v);
						}
					}
				}
			}
		}

		public void VisitUV(int stage, Vector3VisitorCallback callback)
		{
			foreach (var surface in surfaces)
			{
				var gl = surface as ModelBlockGLPrimBase;
				if (gl != null)
				{
					foreach (var i in gl.Indices)
					{
						var v = new Vector3(uv0[i]);
						callback(ref v);
					}
				}
				else
				{
					var prim = surface as ModelBlockPrimBase;
					if (prim != null)
					{
						foreach (var i in prim.Indices)
						{
							var v = new Vector3(uv0[i.UV0]);
							callback(ref v);
						}
					}
				}
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