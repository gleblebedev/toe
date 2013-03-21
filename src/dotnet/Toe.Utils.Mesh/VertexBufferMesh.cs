using System.Collections.Generic;
using System.Drawing;

using OpenTK;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Mesh as set of submeshes with common vertex buffer.
	/// 
	/// The implemenation is not efficient!
	/// Please use it in content pipeline only! It is NOT recommended to use it in production.
	/// </summary>
	public class VertexBufferMesh : SceneItem, IMesh
	{
		#region Constants and Fields

		private readonly List<ISubMesh> submeshes = new List<ISubMesh>();

		private readonly OptimizedList<Vertex> vertexBuffer = new OptimizedList<Vertex>();

		private bool areBoundsValid;

		private Vector3 boundingBoxMax;

		private Vector3 boundingBoxMin;

		private Vector3 boundingSphereCenter;

		private float boundingSphereR;

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

		public int Count
		{
			get
			{
				return this.vertexBuffer.Count;
			}
		}

		public bool IsBinormalStreamAvailable { get; set; }

		public bool IsColorStreamAvailable { get; set; }

		public bool IsNormalStreamAvailable { get; set; }

		public bool IsTangentStreamAvailable { get; set; }

		public bool IsUV0StreamAvailable { get; set; }

		public bool IsUV1StreamAvailable { get; set; }

		public bool IsVertexStreamAvailable
		{
			get
			{
				return true;
			}
		}

		public object RenderData { get; set; }

		public IList<ISubMesh> Submeshes
		{
			get
			{
				return this.submeshes;
			}
		}

		public OptimizedList<Vertex> VertexBuffer
		{
			get
			{
				return this.vertexBuffer;
			}
		}

		public VertexSourceType VertexSourceType
		{
			get
			{
				if (this.submeshes.Count > 0)
				{
					return this.submeshes[0].VertexSourceType;
				}
				return VertexSourceType.TrianleList;
			}
		}

		#endregion

		#region Public Methods and Operators

		public ISubMesh CreateSubmesh()
		{
			var streamSubmesh = new VertexBufferSubmesh(this);
			this.Submeshes.Add(streamSubmesh);
			return streamSubmesh;
		}

		/// <summary>
		/// Get vertex color by index.
		/// </summary>
		/// <param name="index">Vertex index.</param>
		/// <param name="color">Vertex color.</param>
		public void GetColorAt(int index, out Color color)
		{
			color = this.VertexBuffer[index].Color;
		}

		/// <summary>
		/// Get normal position by index.
		/// </summary>
		/// <param name="index">Vertex index.</param>
		/// <param name="vector">Vertex normal.</param>
		public void GetNormalAt(int index, out Vector3 vector)
		{
			vector = this.VertexBuffer[index].Normal;
		}

		/// <summary>
		/// Get vertex texture coords by index.
		/// </summary>
		/// <param name="index">Vertex index.</param>
		/// <param name="channel">Texture channel.</param>
		/// <param name="uv">Vertex UV.</param>
		public void GetUV3At(int index, int channel, out Vector3 uv)
		{
			this.VertexBuffer[index].GetUV(channel, out uv);
		}

		/// <summary>
		/// Get vertex position by index.
		/// </summary>
		/// <param name="index">Vertex index.</param>
		/// <param name="vector">Vertex position.</param>
		public void GetVertexAt(int index, out Vector3 vector)
		{
			vector = this.VertexBuffer[index].Position;
		}

		public void InvalidateBounds()
		{
			this.areBoundsValid = false;
		}

		public void VisitBinormals(Vector3VisitorCallback callback)
		{
			foreach (var vertex in this.VertexBuffer)
			{
				var v = vertex.Binormal;
				callback(ref v);
			}
		}

		public void VisitTangents(Vector3VisitorCallback callback)
		{
			foreach (var vertex in this.VertexBuffer)
			{
				var v = vertex.Tangent;
				callback(ref v);
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
			for (int index = 0; index < this.VertexBuffer.Count; index++)
			{
				var position = this.VertexBuffer[index].Position;
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