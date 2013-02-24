using System.IO;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Meshs file parser interface.
	/// </summary>
	public interface ISceneReader
	{
		#region Public Methods and Operators

		/// <summary>
		/// Load mesh from stream.
		/// </summary>
		/// <param name="stream">Stream to read from.</param>
		/// <param name="basePath">Base path to resources.</param>
		/// <returns>Complete parsed mesh.</returns>
		IScene Load(Stream stream, string basePath);

		#endregion
	}
}