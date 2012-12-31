using System;
using System.Collections.Generic;

namespace Toe.Utils.Mesh
{
	public class VertexBufferMesh : IMesh
	{
		#region Constants and Fields

		private OptimizedList<Vertex> vertexBuffer = new OptimizedList<Vertex>();

		#endregion

		#region Public Properties

		public IList<ISubMesh> Submeshes
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		private string name;
		#endregion

		#region Public Methods and Operators

		public void RenderOpenGL()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}