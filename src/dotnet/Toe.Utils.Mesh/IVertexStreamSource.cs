using System.Collections.Generic;
using System.Drawing;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Vertex stream source.
	/// This is suitable to building vertex buffers.
	/// </summary>
	public interface IVertexStreamSource
	{
		#region Public Properties

		/// <summary>
		/// Number of vertices.
		/// </summary>
		int Count { get; }

		/// <summary>
		/// Gets mesh stream reader if available.
		/// </summary>
		/// <typeparam name="T">Type of stream element.</typeparam>
		/// <param name="key">Stream key.</param>
		/// <param name="channel">Stream channel.</param>
		/// <returns>Read-only list if stream is available, null if not.</returns>
		IList<T> GetStreamReader<T>(string key, int channel);

		#endregion

		bool HasStream(string key, int channel);
	}
}