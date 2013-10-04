using System.Collections.Generic;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Vertex index source.
	/// This is suitable to building vertex buffers.
	/// </summary>
	public interface IVertexIndexSource
	{
		#region Public Properties

		/// <summary>
		/// Type of items.
		/// </summary>
		VertexSourceType VertexSourceType { get; }

		IList<int> GetIndexReader(string key, int channel);

		#endregion
	}
}