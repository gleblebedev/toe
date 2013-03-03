using System;
using System.Collections.Generic;
using System.IO;

namespace Toe.Editors.Interfaces
{
	
	public class FileFormatInfo : IFileFormatInfo
	{
		private readonly Action<string, Stream> create;

		public FileFormatInfo()
		{
		}
		public FileFormatInfo(Action<string, Stream> create)
		{
			this.create = create;
		}

		#region Implementation of IFileFormatInfo

		/// <summary>
		/// Name of file format.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Default file name for a new file.
		/// </summary>
		public string DefaultFileName { get; set; }

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

		/// <summary>
		/// Create new file.
		/// </summary>
		/// <param name="filePath"></param>
		public void Create(string filePath, Stream output)
		{
			if (create == null)
				throw new NotImplementedException();
			create(filePath, output);
		}

		#endregion
	}
}