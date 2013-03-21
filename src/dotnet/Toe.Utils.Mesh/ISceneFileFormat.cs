using System.Collections.Generic;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Meshs file file format.
	/// </summary>
	public interface ISceneFileFormat
	{
		#region Public Properties

		/// <summary>
		/// Scene file format extensions.
		/// </summary>
		IEnumerable<string> Extensions { get; }

		/// <summary>
		/// Scene file format name.
		/// </summary>
		string Name { get; }

		#endregion

		#region Public Methods and Operators

		bool CanLoad(string filename);

		ISceneReader CreateReader();

		#endregion
	}
}