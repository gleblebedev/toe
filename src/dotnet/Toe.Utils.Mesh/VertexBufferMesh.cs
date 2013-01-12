using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Mesh as set of submeshes with common vertex buffer.
	/// 
	/// The implemenation is not efficient!
	/// Please use it in content pipeline only! It is NOT recommended to use it in production.
	/// </summary>
	public class VertexBufferMesh : IMesh
	{
		#region Constants and Fields

		private readonly OptimizedList<Vertex> vertexBuffer = new OptimizedList<Vertex>();

		#endregion

		#region Public Properties

		private readonly List<ISubMesh> submeshes = new List<ISubMesh>();

		public string Name { get; set; }

		public object RenderData
		{
			get; set;
		}

		public IList<ISubMesh> Submeshes
		{
			get
			{
				return submeshes;
			}
		}

		public OptimizedList<Vertex> VertexBuffer
		{
			get
			{
				return this.vertexBuffer;
			}
		}

		#endregion

		/// <summary>
		/// Collection of source specific parameters.
		/// </summary>
		private IParameterCollection parameters;

		#region Implementation of ISceneItem

		/// <summary>
		/// Collection of source specific parameters.
		/// </summary>
		public IParameterCollection Parameters
		{
			get
			{
				return this.parameters ?? (this.parameters = new DynamicCollection());
			}
			set
			{
				this.parameters = value;
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

		#endregion



		#region Implementation of IVertexSource

		public int Count
		{
			get
			{
				return vertexBuffer.Count;
			}
		}

		public bool IsVertexStreamAvailable
		{
			get
			{
				return true;
			}
		}

		public void VisitVertices(Vector3VisitorCallback callback)
		{
			foreach (var vertex in VertexBuffer)
			{
				var v = vertex.Position;
				callback(ref v);
			}
		}

		public void VisitNormals(Vector3VisitorCallback callback)
		{
			foreach (var vertex in VertexBuffer)
			{
				var v = vertex.Normal;
				callback(ref v);
			}
		}

		public void VisitColors(ColorVisitorCallback callback)
		{
			foreach (var vertex in VertexBuffer)
			{
				var v = vertex.Color;
				callback(ref v);
			}
		}

		public void VisitUV(int stage, Vector3VisitorCallback callback)
		{
			if (stage == 0)
			{
				foreach (var vertex in VertexBuffer)
				{
					var v = vertex.UV0;
					callback(ref v);
				}
			}
			else
			{
				foreach (var vertex in VertexBuffer)
				{
					var v = vertex.UV1;
					callback(ref v);
				}
			}
		}

		private bool isNormalStreamAvailable = true;

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

		private bool isBinormalStreamAvailable = true;

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

		private bool isTangentStreamAvailable = true;

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

		private bool isColorStreamAvailable = true;

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

		private bool isUV0StreamAvailable = true;

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

		private bool isUV1StreamAvailable = true;

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

		public VertexSourceType VertexSourceType
		{
			get
			{
				if (submeshes.Count > 0) return submeshes[0].VertexSourceType;
				return VertexSourceType.TrianleList;
			}
		}

		#endregion
	}
}