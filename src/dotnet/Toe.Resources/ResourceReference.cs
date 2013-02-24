using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;

using Toe.Utils;

namespace Toe.Resources
{
	public class ResourceReference : IDisposable, INotifyPropertyChanged
	{
		#region Constants and Fields

		private readonly IBasePathProvider container;

		private readonly IResourceManager resourceManager;

		private readonly uint type;

		private IResourceFileItem consumedFileResource;

		private IResourceItem consumedResource;

		private IResourceFile file;

		private string fileReference;

		private uint hashReference;

		private string nameReference;

		#endregion

		#region Constructors and Destructors

		public ResourceReference(uint type, IResourceManager resourceManager, IBasePathProvider container)
		{
			this.type = type;
			this.resourceManager = resourceManager;
			this.container = container;
		}

		~ResourceReference()
		{
			this.Dispose(false);
		}

		#endregion

		#region Public Events

		public event PropertyChangedEventHandler PropertyChanged;

		public event EventHandler ReferenceChanged;

		#endregion

		#region Public Properties

		public string FilePath
		{
			get
			{
				if (string.IsNullOrEmpty(this.fileReference))
				{
					return null;
				}

				string fullPath;
				if (!string.IsNullOrEmpty(this.Container.BasePath))
				{
					var replace = this.fileReference.Replace('/', Path.DirectorySeparatorChar);
					if (replace.Length > 2 && replace[0] == '.' && replace[1] == Path.DirectorySeparatorChar)
					{
						replace = replace.Substring(2);
					}
					fullPath = Path.Combine(this.Container.BasePath, replace);
				}
				else
				{
					fullPath = this.fileReference;
				}
				return fullPath;
			}
		}

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
					{
						this.file.Close();
					}
					this.fileReference = value;
					if (!string.IsNullOrEmpty(this.fileReference))
					{
						string fullPath;
						fullPath = this.FilePath;
						this.file = this.resourceManager.EnsureFile(fullPath);
						this.file.Open();
						if (this.file.Items != null && this.file.Items.Count > 0)
						{
							var item = this.file.Items[0];
							this.nameReference = item.Name;
							this.hashReference = item.NameHash;
							this.resource = null;
						}
						else
						{
							this.nameReference = null;
							this.hashReference = 0;
							this.resource = null;
						}
					}
					else
					{
						this.nameReference = null;
						this.hashReference = 0;
						this.resource = null;
					}
					this.UpdateConsumedResource();
				}
			}
		}

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
					this.resource = null;
					this.UpdateConsumedResource();
				}
			}
		}

		public bool IsEmpty
		{
			get
			{
				return resource == null && string.IsNullOrEmpty(this.fileReference) && this.hashReference == 0;
			}
		}

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
					this.resource = null;
					if (string.IsNullOrEmpty(this.nameReference))
					{
						this.hashReference = 0;
					}
					else
					{
						this.hashReference = Hash.Get(this.nameReference);
					}
					this.UpdateConsumedResource();
				}
			}
		}

		private object resource = null;
		public object Resource
		{
			get
			{
				if (resource != null) return resource;
				if (this.consumedFileResource != null)
				{
					return this.consumedFileResource.Resource;
				}
				if (this.consumedResource == null)
				{
					return null;
				}
				return this.consumedResource.Value;
			}
			set
			{
				this.fileReference = null;
				this.hashReference = 0;
				this.nameReference = null;
				this.resource = value;
				RaiseReferenceChanged();
			}
		}

		public uint Type
		{
			get
			{
				return this.type;
			}
		}

		public IBasePathProvider Container
		{
			get
			{
				return this.container;
			}
		}

		#endregion

		#region Public Methods and Operators

		public ResourceReference Clone()
		{
			var r = new ResourceReference(this.type, this.resourceManager, this.Container);
			r.fileReference = this.fileReference;
			r.nameReference = this.nameReference;
			r.hashReference = this.hashReference;
			r.resource = this.resource;
			return r;
		}

		public void CopyFrom(ResourceReference newR)
		{
			if (!string.IsNullOrEmpty(newR.fileReference))
			{
				this.FileReference = newR.fileReference;
				return;
			}
			if (!string.IsNullOrEmpty(newR.nameReference))
			{
				this.NameReference = newR.nameReference;
				return;
			}
			this.HashReference = newR.hashReference;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			this.Dispose(true);
		}

		public override string ToString()
		{
			if (!string.IsNullOrEmpty(this.fileReference))
			{
				return Path.GetFileName(this.FileReference);
			}
			if (this.NameReference != null)
			{
				return this.NameReference;
			}
			if (this.hashReference == 0)
			{
				return string.Empty;
			}
			return string.Format(CultureInfo.InvariantCulture, "0x{0:X8}", this.hashReference);
		}

		#endregion

		#region Methods

		protected void RaisePropertyChanged(string property)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(property));
			}
		}

		protected void RaiseReferenceChanged()
		{
			this.RaisePropertyChanged("FileReference");
			this.RaisePropertyChanged("NameReference");
			this.RaisePropertyChanged("HashReference");
			this.RaisePropertyChanged("Resource");

			if (this.ReferenceChanged != null)
			{
				this.ReferenceChanged(this, new EventArgs());
			}
		}

		private void ConsumedResourcePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.RaiseReferenceChanged();
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.FileReference = null;
			}
		}

		private void ReleaseConsumedResource()
		{
			if (this.consumedResource != null)
			{
				var np = this.consumedResource as INotifyPropertyChanged;
				if (np != null)
				{
					np.PropertyChanged -= this.ConsumedResourcePropertyChanged;
				}
				this.resourceManager.ReleaseResource(this.consumedResource.Type, this.consumedResource.Hash);
				this.consumedResource = null;
			}
			this.consumedFileResource = null;
		}

		private void SetConsumedResource(IResourceFileItem resource)
		{
			var obj = this.Resource;
			this.ReleaseConsumedResource();
			this.consumedFileResource = resource;
			if (!Equals(obj, this.Resource))
			{
				this.RaiseReferenceChanged();
			}
		}

		private void SetConsumedResource(IResourceItem resource)
		{
			var obj = this.Resource;
			this.ReleaseConsumedResource();
			this.consumedResource = resource;
			if (this.consumedResource != null)
			{
				var np = this.consumedResource as INotifyPropertyChanged;
				if (np != null)
				{
					np.PropertyChanged += this.ConsumedResourcePropertyChanged;
				}
			}
			if (!Equals(obj, this.Resource))
			{
				this.RaiseReferenceChanged();
			}
		}

		private void UpdateConsumedResource()
		{
			if (this.file != null)
			{
				if (this.file.Items == null)
				{
					this.SetConsumedResource((IResourceItem)null);
					return;
				}
				foreach (IResourceFileItem resourceFileItem in this.file.Items)
				{
					if (resourceFileItem.Type == this.type)
					{
						SetConsumedResource(resourceFileItem);
						return;
					}
				}
				this.SetConsumedResource((IResourceItem)null);
				return;
			}
			if (this.hashReference == 0)
			{
				this.SetConsumedResource((IResourceItem)null);
				return;
			}
			this.SetConsumedResource(this.resourceManager.ConsumeResource(this.type, this.hashReference));
		}

		#endregion
	}
}