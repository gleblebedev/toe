using System.Collections.Generic;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Vertex index source.
	/// This is suitable to building vertex buffers.
	/// </summary>
	public interface IVertexIndexSource : IEnumerable<int>
	{
		#region Public Properties

		/// <summary>
		/// Number of indices.
		/// </summary>
		int Count { get; }

		/// <summary>
		/// Type of items.
		/// </summary>
		VertexSourceType VertexSourceType { get; }

		#endregion
	}
}