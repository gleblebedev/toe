using System.Collections.Generic;

namespace Toe.Editors.Interfaces
{
	public interface IFileFormatInfo
	{
		/// <summary>
		/// Name of file format.
		/// </summary>
		string Name { get; }

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
	}
}