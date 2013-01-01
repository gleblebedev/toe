using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

		public string FilePath
		{
			get
			{
				return this.filePath;
			}
		}

		#region Implementation of IResourceFile

		public void Close()
		{
			--referenceCounter;
			if (referenceCounter == 0)
			{
				//TODO: notify Items property changed
				this.DropResources(this.resources);
				this.resources = null;
			}
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

		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		private void ReadFile()
		{
			try
			{
				this.resourceFileFormat = this.ChooseReader();
				if (this.resourceFileFormat == null) throw new FormatException(string.Format(CultureInfo.InvariantCulture, "Can't read {0}", this.filePath));
				this.DropResources(this.resources);
				this.resources = this.resourceFileFormat.Read(filePath);
				this.ProvideResources(this.resources);

				//TODO: subscribe on list changes
			}
			catch(ThreadAbortException)
			{
				throw;
			}
			catch(Exception ex)
			{
				errorHandler.CanNotRead(filePath, ex);
			}
		}

		private void ProvideResources(IEnumerable<IResourceFileItem> resourceFileItems)
		{
			if (resourceFileItems == null)
				return;
			foreach (var item in resourceFileItems)
			{
				resourceManager.ProvideResource(item.Type, item.NameHash, item.Resource);
				SubscribeOnNameChange(item);
			}
		}

		private void DropResources(IEnumerable<IResourceFileItem> resourceFileItems)
		{
			if (resourceFileItems == null)
				return;
			foreach (var item in resourceFileItems)
			{
				this.UnsubscribeOnNameChange(item);
				resourceManager.RetractResource(item.Type, item.NameHash, item.Resource);
				var disposable = item.Resource as IDisposable;
				if (disposable!=null)
				{
					disposable.Dispose();
				}
			}
			resourceFileItems = null;
		}

		private void SubscribeOnNameChange(IResourceFileItem item)
		{
			item.PropertyChanged += ItemPropertyChanged;
			item.PropertyChanging += ItemPropertyChanging;
		}
		private void UnsubscribeOnNameChange(IResourceFileItem item)
		{
			item.PropertyChanged -= ItemPropertyChanged;
			item.PropertyChanging -= ItemPropertyChanging;
		}
		private void ItemPropertyChanging(object sender, PropertyChangingEventArgs e)
		{
			if (e.PropertyName == "NameHash")
			{
				var item = ((IResourceFileItem)sender);
				resourceManager.RetractResource(item.Type, item.NameHash, item.Resource);
			}
		}

		private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "NameHash")
			{
				var item = ((IResourceFileItem)sender);
				resourceManager.ProvideResource(item.Type, item.NameHash, item.Resource);
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