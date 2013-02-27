using System.Collections.Generic;

namespace Toe.Editors.Interfaces
{
	public interface IResourceEditorFactory
	{
		#region Public Methods and Operators

		/// <summary>
		/// All supported file formats.
		/// </summary>
		IList<IFileFormatInfo> SupportedFormats { get; }

		/// <summary>
		/// Create editor control for specific file.
		/// </summary>
		/// <param name="filename">File name.</param>
		/// <returns>Created editor or null if file format is not supported.</returns>
		IResourceEditor CreateEditor(string filename);

		#endregion
	}
}