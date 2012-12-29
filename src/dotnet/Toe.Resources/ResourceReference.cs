using System;
using System.Globalization;
using System.IO;

namespace Toe.Resources
{
	public class ResourceReference:IDisposable
	{
		~ResourceReference()
		{
			this.Dispose(false);
		}
		public ResourceReference(uint type, IResourceManager resourceManager, IBasePathProvider container)
		{
			this.type = type;
			this.resourceManager = resourceManager;
			this.container = container;
		}

		private uint type;

		private readonly IResourceManager resourceManager;

		private readonly IBasePathProvider container;

		public uint Type
		{
			get
			{
				return this.type;
			}
		}

		private string fileReference;

		private IResourceFile file;

		public string FilePath { get
		{
			if (string.IsNullOrEmpty(this.fileReference)) return null;

			string fullPath;
			if (!string.IsNullOrEmpty(this.container.BasePath))
			{
				var replace = this.fileReference.Replace('/', Path.DirectorySeparatorChar);
				if (replace.Length > 2 && replace[0] == '.' && replace[1] == Path.DirectorySeparatorChar) replace = replace.Substring(2);
				fullPath = Path.Combine(this.container.BasePath, replace);
			}
			else
				fullPath = this.fileReference;
			return fullPath;
		}}

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
					if (this.file != null)
						this.file.Close();
					this.fileReference = value;
					if (!string.IsNullOrEmpty(this.fileReference))
					{
						string fullPath;
						fullPath = FilePath;
						this.file = this.resourceManager.EnsureFile(fullPath);
						this.file.Open();
						if (this.file.Items != null && this.file.Items.Count > 0)
						{
							var item = this.file.Items[0];
							this.nameReference = item.Name;
							this.hashReference = item.NameHash;
						}
						else
						{
							this.nameReference = null;
							this.hashReference = 0;
						}
					}
					else
					{
						this.nameReference = null;
						this.hashReference = 0;
					}
					this.RaiseReferenceChanged();
				}
			}
		}
		protected void RaiseReferenceChanged()
		{
			if (this.ReferenceChanged != null) this.ReferenceChanged(this, new EventArgs());
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
						this.hashReference = 0;
					else 
						this.hashReference = Hash.Get(this.nameReference);
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
			if (this.file != null)
			{
				if (this.file.Items == null)
					return null;
				foreach (IResourceFileItem resourceFileItem in this.file.Items)
				{
					if (resourceFileItem.Type == this.type) return resourceFileItem.Resource;
				}
				return null;
			}
			if (this.hashReference == 0) return null;
			return resourceManager.FindResource(this.type, this.hashReference);
		} }

		public event EventHandler ReferenceChanged;

		public override string ToString()
		{
			if (!string.IsNullOrEmpty(this.fileReference)) return this.FileReference;
			if (this.NameReference != null) return this.NameReference;
			return string.Format(CultureInfo.InvariantCulture, "{0}", this.HashReference);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			this.Dispose(true);
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.FileReference = null;
			}
		}
	}
}