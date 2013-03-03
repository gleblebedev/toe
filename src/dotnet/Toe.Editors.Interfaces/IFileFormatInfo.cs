using System.Collections.Generic;
using System.IO;

namespace Toe.Editors.Interfaces
{
	public interface IFileFormatInfo
	{
		/// <summary>
		/// Name of file format.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Default file name for a new file.
		/// </summary>
		string DefaultFileName { get; }

		/// <summary>
		/// List of extensions with leading dot.
		/// </summary>
		IList<string> Extensions { get; }

		/// <summary>
		/// Is editor able to create new file.
		/// </summary>
		bool CanCreate { get; }

		/// <summary>
		/// Factory.
		/// </summary>
		IResourceEditorFactory Factory { get; }

		/// <summary>
		/// Create new file.
		/// </summary>
		/// <param name="filePath">Output file name.</param>
		/// <param name="output">Output stream.</param>
		void Create(string filePath, Stream output);
	}
}