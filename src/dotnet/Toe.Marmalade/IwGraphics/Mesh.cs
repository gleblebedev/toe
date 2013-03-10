using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using OpenTK;

using Toe.Marmalade.IwGraphics.TangentSpace;
using Toe.Resources;
using Toe.Utils;
using Toe.Utils.Mesh;

namespace Toe.Marmalade.IwGraphics
{
	public class Mesh : Managed, IVertexStreamSource
	{
		#region Constants and Fields

		public static readonly uint TypeHash = Hash.Get("CMesh");

		protected static PropertyEventArgs BaseNameEventArgs = Expr.PropertyEventArgs<Mesh>(x => x.BaseName);

		protected static PropertyEventArgs ScaleEventArgs = Expr.PropertyEventArgs<Mesh>(x => x.Scale);

		protected static PropertyEventArgs UseGeoEventArgs = Expr.PropertyEventArgs<Mesh>(x => x.UseGeo);

		protected static PropertyEventArgs UseGroupEventArgs = Expr.PropertyEventArgs<Mesh>(x => x.UseGroup);

		private readonly MeshStream<Vector3> binormals = new MeshStream<Vector3>();

		private readonly MeshStream<Color> colors = new MeshStream<Color>();

		private readonly MeshStream<Vector3> normals = new MeshStream<Vector3>();

		private readonly IList<Surface> surfaces = new List<Surface>();

		private readonly MeshStream<Vector3> tangents = new MeshStream<Vector3>();

		private readonly MeshStream<Vector2> uv0 = new MeshStream<Vector2>();

		private readonly MeshStream<Vector2> uv1 = new MeshStream<Vector2>();

		private readonly MeshStream<Vector3> vertices = new MeshStream<Vector3>();

		private string baseName;

		private IResourceManager resourceManager;

		private float scale;

		private string useGeo;

		private string useGroup;

		#endregion

		#region Constructors and Destructors

		public Mesh(IResourceManager resourceManager)
		{
			this.resourceManager = resourceManager;
		}

		#endregion

		#region Public Properties

		public string BaseName
		{
			get
			{
				return this.baseName;
			}
			set
			{
				if (this.baseName != value)
				{
					this.RaisePropertyChanging(BaseNameEventArgs.Changing);
					this.baseName = value;
					this.RaisePropertyChanged(BaseNameEventArgs.Changed);
				}
			}
		}

		public MeshStream<Vector3> BiTangents
		{
			get
			{
				return this.binormals;
			}
		}

		public override uint ClassHashCode
		{
			get
			{
				return TypeHash;
			}
		}

		public MeshStream<Color> Colors
		{
			get
			{
				return this.colors;
			}
		}

		public int Count
		{
			get
			{
				return this.surfaces.Sum(submesh => submesh.Count);
			}
		}

		public bool IsBinormalStreamAvailable
		{
			get
			{
				return this.binormals != null && this.binormals.Count > 0;
			}
		}

		public bool IsColorStreamAvailable
		{
			get
			{
				return this.colors != null && this.colors.Count > 0;
			}
		}

		public bool IsNormalStreamAvailable
		{
			get
			{
				return this.normals != null && this.normals.Count > 0;
			}
		}

		public bool IsTangentStreamAvailable
		{
			get
			{
				return this.tangents != null && this.tangents.Count > 0;
			}
		}

		public bool IsUV0StreamAvailable
		{
			get
			{
				return this.uv0 != null && this.uv0.Count > 0;
			}
		}

		public bool IsUV1StreamAvailable
		{
			get
			{
				return this.uv1 != null && this.uv1.Count > 0;
			}
		}

		public bool IsVertexStreamAvailable
		{
			get
			{
				return this.vertices != null && this.vertices.Count > 0;
			}
		}

		public MeshStream<Vector3> Normals
		{
			get
			{
				return this.normals;
			}
		}

		public float Scale
		{
			get
			{
				return this.scale;
			}
			set
			{
				if (this.scale != value)
				{
					this.RaisePropertyChanging(ScaleEventArgs.Changing);
					this.scale = value;
					this.RaisePropertyChanged(ScaleEventArgs.Changed);
				}
			}
		}

		public IList<Surface> Surfaces
		{
			get
			{
				return this.surfaces;
			}
		}

		public MeshStream<Vector3> Tangents
		{
			get
			{
				return this.tangents;
			}
		}

		public MeshStream<Vector2> UV0
		{
			get
			{
				return this.uv0;
			}
		}

		public MeshStream<Vector2> UV1
		{
			get
			{
				return this.uv1;
			}
		}

		public string UseGeo
		{
			get
			{
				return this.useGeo;
			}
			set
			{
				if (this.useGeo != value)
				{
					this.RaisePropertyChanging(UseGeoEventArgs.Changing);
					this.useGeo = value;
					this.RaisePropertyChanged(UseGeoEventArgs.Changed);
				}
			}
		}

		public string UseGroup
		{
			get
			{
				return this.useGroup;
			}
			set
			{
				if (this.useGeo != value)
				{
					this.RaisePropertyChanging(UseGroupEventArgs.Changing);
					this.useGroup = value;
					this.RaisePropertyChanged(UseGroupEventArgs.Changed);
				}
			}
		}

		public VertexSourceType VertexSourceType
		{
			get
			{
				if (this.surfaces.Count > 0)
				{
					return this.surfaces[0].VertexSourceType;
				}
				return VertexSourceType.TrianleList;
			}
		}

		public MeshStream<Vector3> Vertices
		{
			get
			{
				return this.vertices;
			}
		}

		#endregion

		#region Public Methods and Operators

		public void CalculateTangents()
		{
			if (this.tangents != null && this.tangents.Count > 0)
			{
				return;
			}

			var t = new TangentMixer();
			var b = new TangentMixer();
			foreach (var surface in this.Surfaces)
			{
				surface.CalculateTangents(t, b);
			}

			var tt = new Vector3[t.Count];
			foreach (var m in t.Mix.SelectMany(mixes => mixes.Value.Items))
			{
				tt[m.Index] = m.Value;
			}
			var bb = new Vector3[b.Count];
			foreach (var m in b.Mix.SelectMany(mixes => mixes.Value.Items))
			{
				bb[m.Index] = m.Value;
			}

			this.tangents.AddRange(tt);
			this.binormals.AddRange(bb);
		}

		public void VisitBinormals(Vector3VisitorCallback callback)
		{
			foreach (var surface in this.surfaces)
			{
				var gl = surface as ModelBlockGLPrimBase;
				if (gl != null)
				{
					foreach (var i in gl.Indices)
					{
						var v = this.binormals[i];
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
							var v = this.binormals[i.Binormal];
							callback(ref v);
						}
					}
				}
			}
		}

	
		public void VisitTangents(Vector3VisitorCallback callback)
		{
			foreach (var surface in this.surfaces)
			{
				var gl = surface as ModelBlockGLPrimBase;
				if (gl != null)
				{
					foreach (var i in gl.Indices)
					{
						var v = this.tangents[i];
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
							var v = this.tangents[i.Tangent];
							callback(ref v);
						}
					}
				}
			}
		}

		/// <summary>
		/// Get vertex color by index.
		/// </summary>
		/// <param name="index">Vertex index.</param>
		/// <param name="color">Vertex color.</param>
		public void GetColorAt(int index, out Color color)
		{
			foreach (var surface in this.surfaces)
			{
				var gl = surface as ModelBlockGLPrimBase;
				if (gl != null)
				{
					if (index < gl.Indices.Count)
					{
						color = this.colors[gl.Indices[index]];
						return;
					}
					index -= gl.Indices.Count;
				}
				else
				{
					var prim = surface as ModelBlockPrimBase;
					if (prim != null)
					{
						if (index < prim.Indices.Count)
						{
							color = this.colors[prim.Indices[index].Color];
							return;
						}
						index -= prim.Indices.Count;
					}
				}
			}
			throw new IndexOutOfRangeException();
		}

		/// <summary>
		/// Get vertex texture coords by index.
		/// </summary>
		/// <param name="index">Vertex index.</param>
		/// <param name="channel">Texture channel.</param>
		/// <param name="uv">Vertex UV.</param>
		public void GetUV3At(int index, int channel, out Vector3 uv)
		{
			MeshStream<Vector2> meshStream = null;

			switch (channel)
			{
				case 0:
					meshStream = this.uv0;
					break;
				case 1:
					meshStream = this.uv1;
					break;

			}
			foreach (var surface in this.surfaces)
			{
				var gl = surface as ModelBlockGLPrimBase;
				if (gl != null)
				{
					if (index < gl.Indices.Count)
					{
						var a = meshStream[gl.Indices[index]];
						uv = new Vector3(a.X,a.Y,0);
						return;
					}
					index -= gl.Indices.Count;
				}
				else
				{
					var prim = surface as ModelBlockPrimBase;
					if (prim != null)
					{
						if (index < prim.Indices.Count)
						{
							var a = meshStream[prim.Indices[index].GetUV(channel)];
							uv = new Vector3(a.X, a.Y, 0);
							return;
						}
						index -= prim.Indices.Count;
					}
				}
			}
			throw new IndexOutOfRangeException();
		}

		/// <summary>
		/// Get vertex position by index.
		/// </summary>
		/// <param name="index">Vertex index.</param>
		/// <param name="vector">Vertex position.</param>
		public void GetVertexAt(int index, out Vector3 vector)
		{
			foreach (var surface in this.surfaces)
			{
				var gl = surface as ModelBlockGLPrimBase;
				if (gl != null)
				{
					if (index < gl.Indices.Count)
					{
						vector = this.vertices[gl.Indices[index]];
						return;
					}
					index -= gl.Indices.Count;
				}
				else
				{
					var prim = surface as ModelBlockPrimBase;
					if (prim != null)
					{
						if (index < prim.Indices.Count)
						{
							vector = this.vertices[prim.Indices[index].Vertex];
							return;
						}
						index -= prim.Indices.Count;
					}
				}
			}
			throw new IndexOutOfRangeException();
		}

		/// <summary>
		/// Get normal position by index.
		/// </summary>
		/// <param name="index">Vertex index.</param>
		/// <param name="vector">Vertex normal.</param>
		public void GetNormalAt(int index, out Vector3 vector)
		{
			foreach (var surface in this.surfaces)
			{
				var gl = surface as ModelBlockGLPrimBase;
				if (gl != null)
				{
					if (index < gl.Indices.Count)
					{
						vector = this.normals[gl.Indices[index]];
						return;
					}
					index -= gl.Indices.Count;
				}
				else
				{
					var prim = surface as ModelBlockPrimBase;
					if (prim != null)
					{
						if (index < prim.Indices.Count)
						{
							vector = this.normals[prim.Indices[index].Normal];
							return;
						}
						index -= prim.Indices.Count;
					}
				}
			}
			throw new IndexOutOfRangeException();
		}

		

		#endregion
	}
}