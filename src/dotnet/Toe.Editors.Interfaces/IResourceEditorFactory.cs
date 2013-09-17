using System.Collections.Generic;

namespace Toe.Editors.Interfaces
{
	public interface IResourceEditorFactory
	{
		#region Public Properties

		/// <summary>
		/// Name of resource editor group.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// All supported file formats.
		/// </summary>
		IList<IFileFormatInfo> SupportedFormats { get; }

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Create editor control for specific file.
		/// </summary>
		/// <param name="filename">File name.</param>
		/// <returns>Created editor or null if file format is not supported.</returns>
		IResourceEditor CreateEditor(string filename);

		#endregion
	}

}