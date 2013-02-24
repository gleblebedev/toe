using System.Collections.Generic;

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

		private bool isBinormalStreamAvailable = false;

		private bool isColorStreamAvailable = false;

		private bool isNormalStreamAvailable = false;

		private bool isTangentStreamAvailable = false;

		private bool isUV0StreamAvailable = false;

		private bool isUV1StreamAvailable = false;

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

		public bool IsBinormalStreamAvailable
		{
			get
			{
				return this.isBinormalStreamAvailable;
			}
			set
			{
				this.isBinormalStreamAvailable = value;
			}
		}

		public bool IsColorStreamAvailable
		{
			get
			{
				return this.isColorStreamAvailable;
			}
			set
			{
				this.isColorStreamAvailable = value;
			}
		}

		public bool IsNormalStreamAvailable
		{
			get
			{
				return this.isNormalStreamAvailable;
			}
			set
			{
				this.isNormalStreamAvailable = value;
			}
		}

		public bool IsTangentStreamAvailable
		{
			get
			{
				return this.isTangentStreamAvailable;
			}
			set
			{
				this.isTangentStreamAvailable = value;
			}
		}

		public bool IsUV0StreamAvailable
		{
			get
			{
				return this.isUV0StreamAvailable;
			}
			set
			{
				this.isUV0StreamAvailable = value;
			}
		}

		public bool IsUV1StreamAvailable
		{
			get
			{
				return this.isUV1StreamAvailable;
			}
			set
			{
				this.isUV1StreamAvailable = value;
			}
		}

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

		public void VisitColors(ColorVisitorCallback callback)
		{
			foreach (var vertex in this.VertexBuffer)
			{
				var v = vertex.Color;
				callback(ref v);
			}
		}

		public void VisitNormals(Vector3VisitorCallback callback)
		{
			foreach (var vertex in this.VertexBuffer)
			{
				var v = vertex.Normal;
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

		public void VisitUV(int stage, Vector3VisitorCallback callback)
		{
			if (stage == 0)
			{
				foreach (var vertex in this.VertexBuffer)
				{
					var v = vertex.UV0;
					callback(ref v);
				}
			}
			else
			{
				foreach (var vertex in this.VertexBuffer)
				{
					var v = vertex.UV1;
					callback(ref v);
				}
			}
		}

		public void VisitVertices(Vector3VisitorCallback callback)
		{
			foreach (var vertex in this.VertexBuffer)
			{
				var v = vertex.Position;
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

			boundingBoxMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
			boundingBoxMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);
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
			boundingSphereCenter = (boundingBoxMax + boundingBoxMin) * 0.5f;
			boundingSphereR = (boundingBoxMax - boundingBoxMin).Length * 0.5f;
		}

		#endregion
	}
}