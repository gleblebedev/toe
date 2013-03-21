using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using OpenTK;

#if WINDOWS_PHONE
using Microsoft.Xna.Framework;
#else

#endif

namespace Toe.Utils.Mesh
{
	public class StreamMesh : SceneItem, IMesh
	{
		#region Constants and Fields

		public MeshStream<VertexWeight> weights = new MeshStream<VertexWeight>();

		private readonly MeshStream<Vector3> binormals = new MeshStream<Vector3>();

		private readonly BoneCollection bones = new BoneCollection();

		private readonly MeshStream<Color> colors = new MeshStream<Color>();

		private readonly MeshStream<Vector3> normals = new MeshStream<Vector3>();

		private readonly List<ISubMesh> submeshes = new List<ISubMesh>();

		private readonly MeshStream<Vector3> tangents = new MeshStream<Vector3>();

		private readonly List<MeshStream<Vector3>> uv = new List<MeshStream<Vector3>>();

		private readonly MeshStream<Vector3> vertices = new MeshStream<Vector3>();

		private bool areBoundsValid;

		private Vector3 boundingBoxMax;

		private Vector3 boundingBoxMin;

		private Vector3 boundingSphereCenter;

		private float boundingSphereR;

		#endregion

		#region Public Properties

		public string BaseName { get; set; }

		public MeshStream<Vector3> Binormals
		{
			get
			{
				return this.binormals;
			}
		}

		public BoneCollection Bones
		{
			get
			{
				return this.bones;
			}
		}

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
				return this.submeshes.Sum(submesh => submesh.Count);
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
				return this.uv.Count > 0 && this.uv[0].Count > 0;
			}
		}

		public bool IsUV1StreamAvailable
		{
			get
			{
				return this.uv.Count > 1 && this.uv[1].Count > 0;
			}
		}

		public bool IsVertexStreamAvailable
		{
			get
			{
				return this.vertices != null && this.vertices.Count > 0;
			}
		}

		public uint NameHash { get; set; }

		public MeshStream<Vector3> Normals
		{
			get
			{
				return this.normals;
			}
		}

		public object RenderData { get; set; }

		public float Scale { get; set; }

		public string Skeleton { get; set; }

		public string SkeletonModel { get; set; }

		public IList<ISubMesh> Submeshes
		{
			get
			{
				return this.submeshes;
			}
		}

		public MeshStream<Vector3> Tangents
		{
			get
			{
				return this.tangents;
			}
		}

		public IList<MeshStream<Vector3>> UV
		{
			get
			{
				return this.uv;
			}
		}

		public MeshStream<Vector3> Vertices
		{
			get
			{
				return this.vertices;
			}
		}

		public MeshStream<VertexWeight> Weights
		{
			get
			{
				return this.weights;
			}
		}

		public string useGeo { get; set; }

		public string useGroup { get; set; }

		#endregion

		#region Public Methods and Operators

		public ISubMesh CreateSubmesh()
		{
			var streamSubmesh = new StreamSubmesh(this);
			this.Submeshes.Add(streamSubmesh);
			return streamSubmesh;
		}

		public int EnsureBone(string boneName)
		{
			return this.bones.EnsureBone(boneName);
		}

		public MeshStream<Vector3> EnsureUVStream(int setId)
		{
			while (this.UV.Count <= setId)
			{
				this.UV.Add(new MeshStream<Vector3>());
			}
			return this.UV[setId];
		}

		/// <summary>
		/// Get vertex color by index.
		/// </summary>
		/// <param name="index">Vertex index.</param>
		/// <param name="color">Vertex color.</param>
		public void GetColorAt(int index, out Color color)
		{
			foreach (StreamSubmesh submesh in this.submeshes)
			{
				if (index < submesh.Indices.Count)
				{
					color = this.colors[submesh.Indices[index].Color];
					return;
				}
				index -= submesh.Indices.Count;
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
			foreach (StreamSubmesh submesh in this.submeshes)
			{
				if (index < submesh.Indices.Count)
				{
					vector = this.normals[submesh.Indices[index].Normal];
					return;
				}
				index -= submesh.Indices.Count;
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
			foreach (StreamSubmesh submesh in this.submeshes)
			{
				if (index < submesh.Indices.Count)
				{
					uv = this.uv[channel][submesh.Indices[index].GetUV(channel)];
					return;
				}
				index -= submesh.Indices.Count;
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
			foreach (StreamSubmesh submesh in this.submeshes)
			{
				if (index < submesh.Indices.Count)
				{
					vector = this.vertices[submesh.Indices[index].Vertex];
					return;
				}
				index -= submesh.Indices.Count;
			}
			throw new IndexOutOfRangeException();
		}

		public void InvalidateBounds()
		{
			this.areBoundsValid = false;
		}

		public void VisitBinormals(Vector3VisitorCallback callback)
		{
			foreach (StreamSubmesh submesh in this.submeshes)
			{
				foreach (var index in submesh.Indices)
				{
					var v = this.binormals[index.Binormal];
					callback(ref v);
				}
			}
		}

		public void VisitNormals(Vector3VisitorCallback callback)
		{
			foreach (StreamSubmesh submesh in this.submeshes)
			{
				foreach (var index in submesh.Indices)
				{
					var v = this.normals[index.Vertex];
					callback(ref v);
				}
			}
		}

		public void VisitTangents(Vector3VisitorCallback callback)
		{
			foreach (StreamSubmesh submesh in this.submeshes)
			{
				foreach (var index in submesh.Indices)
				{
					var v = this.tangents[index.Tangent];
					callback(ref v);
				}
			}
		}

		#endregion

		#region Methods

		protected void CalculateBounds()
		{
			if (this.areBoundsValid)
			{
				return;
			}
			this.areBoundsValid = true;
			this.boundingBoxMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
			this.boundingBoxMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);
			foreach (var vector3 in this.Vertices)
			{
				if (this.boundingBoxMax.X < vector3.X)
				{
					this.boundingBoxMax.X = vector3.X;
				}
				if (this.boundingBoxMax.Y < vector3.Y)
				{
					this.boundingBoxMax.Y = vector3.Y;
				}
				if (this.boundingBoxMax.Z < vector3.Z)
				{
					this.boundingBoxMax.Z = vector3.Z;
				}
				if (this.boundingBoxMin.X > vector3.X)
				{
					this.boundingBoxMin.X = vector3.X;
				}
				if (this.boundingBoxMin.Y > vector3.Y)
				{
					this.boundingBoxMin.Y = vector3.Y;
				}
				if (this.boundingBoxMin.Z > vector3.Z)
				{
					this.boundingBoxMin.Z = vector3.Z;
				}
			}
			this.boundingSphereCenter = (this.boundingBoxMax + this.boundingBoxMin) * 0.5f;
			this.boundingSphereR = (this.boundingBoxMax - this.boundingBoxMin).Length * 0.5f;
		}

		#endregion
	}
}