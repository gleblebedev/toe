using System;
using System.Collections.Generic;
using System.IO;

namespace Toe.Editors.Interfaces
{
	public class FileFormatInfo : IFileFormatInfo
	{
		#region Constants and Fields

		private readonly Action<string, Stream> create;

		#endregion

		#region Constructors and Destructors

		public FileFormatInfo()
		{
		}

		public FileFormatInfo(Action<string, Stream> create)
		{
			this.create = create;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Is editor able to create new file.
		/// </summary>
		public bool CanCreate { get; set; }

		/// <summary>
		/// Default file name for a new file.
		/// </summary>
		public string DefaultFileName { get; set; }

		/// <summary>
		/// List of extensions with leading dot.
		/// </summary>
		public IList<string> Extensions { get; set; }

		/// <summary>
		/// Factory.
		/// </summary>
		public IResourceEditorFactory Factory { get; set; }

		/// <summary>
		/// Name of file format.
		/// </summary>
		public string Name { get; set; }

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Create new file.
		/// </summary>
		/// <param name="filePath"></param>
		public void Create(string filePath, Stream output)
		{
			if (this.create == null)
			{
				throw new NotImplementedException();
			}
			this.create(filePath, output);
		}

		#endregion
	}
}