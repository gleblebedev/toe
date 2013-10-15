using System;
using System.Collections.Generic;
using System.Drawing;

using Toe.Utils.ToeMath;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Mesh as set of submeshes with common vertex buffer.
	/// 
	/// The implemenation is not efficient!
	/// Please use it in content pipeline only! It is NOT recommended to use it in production.
	/// </summary>
	public class VertexBufferMesh<TVertex> : SceneItem, IMesh
	{
		#region Constants and Fields

		private readonly List<ISubMesh> submeshes = new List<ISubMesh>();

		private readonly DictionaryMeshStream<TVertex> vertexBuffer = new DictionaryMeshStream<TVertex>();

		private bool areBoundsValid;

		private Float3 boundingBoxMax;

		private Float3 boundingBoxMin;

		private Float3 boundingSphereCenter;

		private float boundingSphereR;

		#endregion

		#region Public Properties

		public Float3 BoundingBoxMax
		{
			get
			{
				this.CalculateBounds();
				return this.boundingBoxMax;
			}
		}

		public Float3 BoundingBoxMin
		{
			get
			{
				this.CalculateBounds();
				return this.boundingBoxMin;
			}
		}

		public Float3 BoundingSphereCenter
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

		/// <summary>
		/// Gets mesh stream reader if available.
		/// </summary>
		/// <typeparam name="T">Type of stream element.</typeparam>
		/// <param name="key">Stream key.</param>
		/// <param name="channel">Stream channel.</param>
		/// <returns>Stream reader if available, null if not.</returns>
		public IList<T> GetStreamReader<T>(string key, int channel)
		{
			throw new NotImplementedException();
		}

		public bool HasStream(string key, int channel)
		{
			throw new NotImplementedException();
		}


		public object RenderData { get; set; }

		public IList<ISubMesh> Submeshes
		{
			get
			{
				return this.submeshes;
			}
		}

		public DictionaryMeshStream<TVertex> VertexBuffer
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
				return VertexSourceType.TriangleList;
			}
		}

		#endregion

		#region Public Methods and Operators

		public ISubMesh CreateSubmesh()
		{
			var streamSubmesh = new VertexBufferSubmesh<TVertex>(this);
			this.Submeshes.Add(streamSubmesh);
			return streamSubmesh;
		}

		public void InvalidateBounds()
		{
			this.areBoundsValid = false;
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

			this.boundingBoxMin = new Float3(float.MaxValue, float.MaxValue, float.MaxValue);
			this.boundingBoxMax = new Float3(float.MinValue, float.MinValue, float.MinValue);
			foreach (var position in GetStreamReader<Float3>(Streams.Position, 0))
			{
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