using System.Collections.Generic;

namespace Toe.Editors.Interfaces
{
	public class FileFormatInfo : IFileFormatInfo
	{
		#region Implementation of IFileFormatInfo

		/// <summary>
		/// Name of file format.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// List of extensions with leading dot.
		/// </summary>
		public IList<string> Extensions
		{
			get;
			set;
		}

		/// <summary>
		/// Is editor able to create new file.
		/// </summary>
		public bool CanCreate
		{
			get;
			set;
		}

		/// <summary>
		/// Factory.
		/// </summary>
		public IResourceEditorFactory Factory
		{
			get;
			set;
		}

		#endregion
	}
}