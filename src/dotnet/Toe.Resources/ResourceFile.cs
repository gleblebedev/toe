using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;

using Autofac;

namespace Toe.Resources
{
	public class ResourceFile : IResourceFile
	{
		public override string ToString()
		{
			return filePath;
		}

		private readonly IResourceManager resourceManager;

		private readonly string filePath;

		private readonly IEnumerable<IResourceFileFormat> readers;

		private readonly IResourceErrorHandler errorHandler;

		private readonly IComponentContext context;

		private int referenceCounter;

		private IResourceFileFormat resourceFileFormat;

		private IList<IResourceFileItem> resources;

		public ResourceFile(IResourceManager resourceManager, string filePath, IEnumerable<IResourceFileFormat> readers, IResourceErrorHandler errorHandler)
		{
			this.resourceManager = resourceManager;
			this.filePath = filePath;
			this.readers = readers;
			this.errorHandler = errorHandler;
			this.context = context;
		}

		public IList<IResourceFileItem> Items
		{
			get
			{
				return this.resources;
			}
			set
			{
				this.resources = value;
			}
		}

		#region Implementation of IResourceFile

		public void Close()
		{
			--referenceCounter;
		}

		public void Open()
		{
			++referenceCounter;
			if (referenceCounter == 1)
			{
				ReadFile();
			}
		}

		#endregion

		private void ReadFile()
		{
			try
			{
				this.resourceFileFormat = this.ChooseReader();
				if (this.resourceFileFormat == null) throw new FormatException(string.Format(CultureInfo.InvariantCulture, "Can't read {0}", this.filePath));
				this.resources = this.resourceFileFormat.Read(filePath);
			}
			catch(Exception ex)
			{
				errorHandler.CanNotRead(filePath, ex);
			}
		}

		private IResourceFileFormat ChooseReader()
		{
			foreach (var reader in this.readers)
			{
				if (reader.CanRead(filePath))
				{
					return reader;
				}
			}
			return null;
		}
	}
}