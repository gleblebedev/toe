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

		public uint NameHash { get; set; }

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
			return this.submeshes.SelectMany(submesh => submesh).GetEnumerator();
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
				return true;
			}
		}

		public bool IsNormalStreamAvailable
		{
			get
			{
				return true;
			}
		}

		public bool IsBinormalStreamAvailable
		{
			get
			{
				return true;
			}
		}

		public bool IsTangentStreamAvailable
		{
			get
			{
				return true;
			}
		}

		public bool IsColorStreamAvailable
		{
			get
			{
				return true;
			}
		}

		public bool IsUV0StreamAvailable
		{
			get
			{
				return true;
			}
		}

		public bool IsUV1StreamAvailable
		{
			get
			{
				return true;
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