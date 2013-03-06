using System.Collections.Generic;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Meshs file file format.
	/// </summary>
	public interface ISceneFileFormat
	{
		#region Public Methods and Operators

		/// <summary>
		/// Scene file format name.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Scene file format extensions.
		/// </summary>
		IEnumerable<string> Extensions { get; }

		bool CanLoad(string filename);

		ISceneReader CreateReader();

		#endregion
	}
}