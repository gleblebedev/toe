using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Threading;

using Autofac;

namespace Toe.Resources
{
	public class ResourceFile : IResourceFile
	{
		#region Constants and Fields

		private readonly IComponentContext context;

		private readonly IResourceErrorHandler errorHandler;

		private readonly string filePath;

		private readonly IEnumerable<IResourceFileFormat> readers;

		private readonly IResourceManager resourceManager;

		private int referenceCounter;

		private IResourceFileFormat resourceFileFormat;

		private IList<IResourceFileItem> resources;

		#endregion

		#region Constructors and Destructors

		public ResourceFile(
			IResourceManager resourceManager,
			string filePath,
			IEnumerable<IResourceFileFormat> readers,
			IResourceErrorHandler errorHandler)
		{
			this.resourceManager = resourceManager;
			this.filePath = filePath;
			this.readers = readers;
			this.errorHandler = errorHandler;
			this.context = this.context;
		}

		#endregion

		#region Public Properties

		public string BasePath
		{
			get
			{
				return Path.GetDirectoryName(this.filePath);
			}
		}

		public string FilePath
		{
			get
			{
				return this.filePath;
			}
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

		#endregion

		#region Public Methods and Operators

		public void Close()
		{
			--this.referenceCounter;
			if (this.referenceCounter == 0)
			{
				//TODO: notify Items property changed
				this.DropResources(this.resources);
				this.resources = null;
			}
		}

		public void Open()
		{
			++this.referenceCounter;
			if (this.referenceCounter == 1)
			{
				this.ReadFile();
			}
		}

		public override string ToString()
		{
			return this.filePath;
		}

		#endregion

		#region Methods

		private IResourceFileFormat ChooseReader()
		{
			foreach (var reader in this.readers)
			{
				if (reader.CanRead(this.filePath))
				{
					return reader;
				}
			}
			return null;
		}

		private void DropResources(IEnumerable<IResourceFileItem> resourceFileItems)
		{
			if (resourceFileItems == null)
			{
				return;
			}
			foreach (var item in resourceFileItems)
			{
				this.UnsubscribeOnNameChange(item);
				this.resourceManager.RetractResource(item.Type, item.NameHash, item.Resource, this);
				var disposable = item.Resource as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			resourceFileItems = null;
		}

		private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "NameHash")
			{
				var item = ((IResourceFileItem)sender);
				this.resourceManager.ProvideResource(item.Type, item.NameHash, item.Resource, this);
			}
		}

		private void ItemPropertyChanging(object sender, PropertyChangingEventArgs e)
		{
			if (e.PropertyName == "NameHash")
			{
				var item = ((IResourceFileItem)sender);
				this.resourceManager.RetractResource(item.Type, item.NameHash, item.Resource, this);
			}
		}

		private void ProvideResources(IEnumerable<IResourceFileItem> resourceFileItems)
		{
			if (resourceFileItems == null)
			{
				return;
			}
			foreach (var item in resourceFileItems)
			{
				this.resourceManager.ProvideResource(item.Type, item.NameHash, item.Resource, this);
				this.SubscribeOnNameChange(item);
			}
		}

		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		private void ReadFile()
		{
			try
			{
				this.resourceFileFormat = this.ChooseReader();
				if (this.resourceFileFormat == null)
				{
					throw new FormatException(
						string.Format(CultureInfo.InvariantCulture, "Can't find a reader for {0}", this.filePath));
				}
				this.DropResources(this.resources);
				this.resources = this.resourceFileFormat.Read(this.filePath, this);

				//TODO: subscribe on list changes
			}
			catch (ThreadAbortException)
			{
				this.DropResources(this.resources);
				throw;
			}
			catch (Exception ex)
			{
				this.errorHandler.CanNotRead(this.filePath, ex);
			}
			this.ProvideResources(this.resources);
		}

		private void SubscribeOnNameChange(IResourceFileItem item)
		{
			item.PropertyChanged += this.ItemPropertyChanged;
			item.PropertyChanging += this.ItemPropertyChanging;
		}

		private void UnsubscribeOnNameChange(IResourceFileItem item)
		{
			item.PropertyChanged -= this.ItemPropertyChanged;
			item.PropertyChanging -= this.ItemPropertyChanging;
		}

		#endregion
	}
}