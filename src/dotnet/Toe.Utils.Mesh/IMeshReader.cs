using System.IO;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Meshs file parser interface.
	/// </summary>
	public interface IMeshReader
	{
		#region Public Methods and Operators

		/// <summary>
		/// Load mesh from stream.
		/// </summary>
		/// <param name="stream">Stream to read from.</param>
		/// <returns>Complete parsed mesh.</returns>
		IMesh Load(Stream stream);

		#endregion
	}
}