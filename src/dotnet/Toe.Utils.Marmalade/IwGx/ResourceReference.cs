using System;
using System.Globalization;
using System.IO;

using Toe.Resources;

namespace Toe.Utils.Mesh.Marmalade.IwGx
{
	public class ResourceReference:IDisposable
	{
		~ResourceReference()
		{
			Dispose(false);
		}
		public ResourceReference(uint type, IResourceManager resourceManager, Managed container)
		{
			this.type = type;
			this.resourceManager = resourceManager;
			this.container = container;
		}

		private uint type;

		private readonly IResourceManager resourceManager;

		private readonly Managed container;

		public uint Type
		{
			get
			{
				return this.type;
			}
		}

		private string fileReference;

		private IResourceFile file;

		public string FileReference
		{
			get
			{
				return this.fileReference;
			}
			set
			{
				if (this.fileReference != value)
				{
					if (file != null)
						file.Close();
					this.fileReference = value;
					if (!string.IsNullOrEmpty(fileReference))
					{
							string fullPath;
						if (!string.IsNullOrEmpty(container.BasePath))
						{
							var replace = this.fileReference.Replace('/', Path.DirectorySeparatorChar);
							if (replace.Length > 2 && replace[0] == '.' && replace[1] == Path.DirectorySeparatorChar) replace = replace.Substring(2);
							fullPath = Path.Combine(this.container.BasePath, replace);
						}
						else
							fullPath =  fileReference;
						file = resourceManager.EnsureFile(fullPath);
						file.Open();
						if (file.Items != null && file.Items.Count > 0)
						{
							var item = file.Items[0];
							nameReference = item.Name;
							hashReference = item.NameHash;
						}
						else
						{
							nameReference = null;
							hashReference = 0;
						}
					}
					else
					{
						nameReference = null;
						hashReference = 0;
					}
					this.RaiseReferenceChanged();
				}
			}
		}
		protected void RaiseReferenceChanged()
		{
			if (ReferenceChanged != null) ReferenceChanged(this, new EventArgs());
		}

		private string nameReference;

		public string NameReference
		{
			get
			{
				return this.nameReference;
			}
			set
			{
				if (this.nameReference != value)
				{
					this.nameReference = value;
					if (string.IsNullOrEmpty(this.nameReference))
						hashReference = 0;
					else 
						hashReference = Hash.Get(nameReference);
					this.RaiseReferenceChanged();
				}
			}
		}

		private uint hashReference;

		public uint HashReference
		{
			get
			{
				return this.hashReference;
			}
			set
			{
				if (this.hashReference != value)
				{
					this.FileReference = null;
					this.hashReference = value;
					this.RaiseReferenceChanged();
				}
			}
		}

		public object Resource { get
		{
			if (file != null) return file.Items[0].Resource;
			if (hashReference == 0) return null;
			throw new NotImplementedException();
		} }

		public event EventHandler ReferenceChanged;

		public override string ToString()
		{
			if (!string.IsNullOrEmpty(fileReference)) return FileReference;
			if (NameReference != null) return NameReference;
			return string.Format(CultureInfo.InvariantCulture, "{0}", HashReference);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			Dispose(true);
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				FileReference = null;
			}
		}
	}
}