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

		private readonly ListMeshStream<Vector3> binormals = new ListMeshStream<Vector3>();

		private readonly ListMeshStream<Color> colors = new ListMeshStream<Color>();

		private readonly ListMeshStream<Vector3> normals = new ListMeshStream<Vector3>();

		private readonly IList<Surface> surfaces = new List<Surface>();

		private readonly ListMeshStream<Vector3> tangents = new ListMeshStream<Vector3>();

		private readonly ListMeshStream<Vector2> uv0 = new ListMeshStream<Vector2>();

		private readonly ListMeshStream<Vector2> uv1 = new ListMeshStream<Vector2>();

		private readonly ListMeshStream<Vector3> vertices = new ListMeshStream<Vector3>();

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

		public ListMeshStream<Vector3> BiTangents
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

		public ListMeshStream<Color> Colors
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

		/// <summary>
		/// Gets mesh stream reader if available.
		/// </summary>
		/// <typeparam name="T">Type of stream element.</typeparam>
		/// <param name="key">Stream key.</param>
		/// <param name="channel">Stream channel.</param>
		/// <returns>Stream reader if available, null if not.</returns>
		public IList<T> GetStreamReader<T>(string key, int channel)
		{
			if (key == Streams.Position)
			{
				return this.GetStreamReader<T>(this.Vertices);
			}
			if (key == Streams.Normal)
			{
				return this.GetStreamReader<T>(this.Normals);
			}
			if (key == Streams.TexCoord && channel == 0)
			{
				return this.GetStreamReader<T>(this.UV0);
			}
			if (key == Streams.TexCoord && channel == 1)
			{
				return this.GetStreamReader<T>(this.UV1);
			}
			if (key == Streams.Binormal && channel == 0)
			{
				return this.GetStreamReader<T>(this.BiTangents);
			}
			if (key == Streams.Tangent && channel == 0)
			{
				return this.GetStreamReader<T>(this.Tangents);
			}
			if (key == Streams.Color && channel == 0)
			{
				return this.GetStreamReader<T>(this.Colors);
			}
			return null;
		}

		IStreamConverterFactory streamConverterFactory = StreamConverterFactory.Default;

		private IList<T> GetStreamReader<T>(ListMeshStream<Vector3> key)
		{
			if (key == null || key.Count == 0) return null;
			if (typeof(T) == typeof(Vector3)) return (IList<T>)key;
			var resolveConverter = streamConverterFactory.ResolveConverter<Vector3, T>(key);
			if (resolveConverter != null) return resolveConverter;
			throw new NotImplementedException();
		}
		private IList<T> GetStreamReader<T>(ListMeshStream<Vector2> key)
		{
			if (key == null || key.Count == 0) return null;
			if (typeof(T) == typeof(Vector2)) return (IList<T>)key;
			var resolveConverter = streamConverterFactory.ResolveConverter<Vector2, T>(key);
			if (resolveConverter != null) return resolveConverter;
			throw new NotImplementedException();
		}
		private IList<T> GetStreamReader<T>(ListMeshStream<Color> key)
		{
			if (key == null || key.Count == 0) return null;
			if (typeof(T) == typeof(Color)) return (IList<T>)key;
			var resolveConverter = streamConverterFactory.ResolveConverter<Color, T>(key);
			if (resolveConverter != null) return resolveConverter;
			throw new NotImplementedException();
		}
		public bool HasStream(string key, int channel)
		{
			if (key == Streams.Position)
			{
				return this.Vertices != null;
			}
			if (key == Streams.Normal)
			{
				return this.Normals != null;
			}
			if (key == Streams.TexCoord && channel == 0)
			{
				return this.UV0 != null;
			}
			if (key == Streams.TexCoord && channel == 1)
			{
				return this.UV1 != null;
			}
			if (key == Streams.Binormal && channel == 0)
			{
				return this.BiTangents != null;
			}
			if (key == Streams.Tangent && channel == 0)
			{
				return this.Tangents != null;
			}
			if (key == Streams.Color && channel == 0)
			{
				return this.Colors != null;
			}
			return false;
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

		public ListMeshStream<Vector3> Normals
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

		public ListMeshStream<Vector3> Tangents
		{
			get
			{
				return this.tangents;
			}
		}

		public ListMeshStream<Vector2> UV0
		{
			get
			{
				return this.uv0;
			}
		}

		public ListMeshStream<Vector2> UV1
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
				return VertexSourceType.TriangleList;
			}
		}

		public ListMeshStream<Vector3> Vertices
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

		/// <summary>
		/// Get vertex texture coords by index.
		/// </summary>
		/// <param name="index">Vertex index.</param>
		/// <param name="channel">Texture channel.</param>
		/// <param name="uv">Vertex UV.</param>
		public void GetUV3At(int index, int channel, out Vector3 uv)
		{
			ListMeshStream<Vector2> listMeshStream = null;

			switch (channel)
			{
				case 0:
					listMeshStream = this.uv0;
					break;
				case 1:
					listMeshStream = this.uv1;
					break;
			}
			foreach (var surface in this.surfaces)
			{
				var gl = surface as ModelBlockGLPrimBase;
				if (gl != null)
				{
					if (index < gl.Indices.Count)
					{
						var a = listMeshStream[gl.Indices[index]];
						uv = new Vector3(a.X, a.Y, 0);
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
							var a = listMeshStream[prim.Indices[index].GetUV(channel)];
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

	

		#endregion
	}
}