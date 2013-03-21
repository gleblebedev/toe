using System.Collections.Generic;
using System.IO;

namespace Toe.Editors.Interfaces
{
	public interface IFileFormatInfo
	{
		#region Public Properties

		/// <summary>
		/// Is editor able to create new file.
		/// </summary>
		bool CanCreate { get; }

		/// <summary>
		/// Default file name for a new file.
		/// </summary>
		string DefaultFileName { get; }

		/// <summary>
		/// List of extensions with leading dot.
		/// </summary>
		IList<string> Extensions { get; }

		/// <summary>
		/// Factory.
		/// </summary>
		IResourceEditorFactory Factory { get; }

		/// <summary>
		/// Name of file format.
		/// </summary>
		string Name { get; }

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Create new file.
		/// </summary>
		/// <param name="filePath">Output file name.</param>
		/// <param name="output">Output stream.</param>
		void Create(string filePath, Stream output);

		#endregion
	}
}