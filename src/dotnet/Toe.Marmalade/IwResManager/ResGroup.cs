using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

using Autofac;

using Toe.Resources;

namespace Toe.Marmalade.IwResManager
{
	public class ResGroup : Managed
	{
		#region Constants and Fields

		public static readonly uint TypeHash = Hash.Get("CIwResGroup");

		private readonly IComponentContext context;

		private readonly IList<Managed> embeddedResources;

		private readonly IList<IResourceFile> externalResources;

		private readonly IResourceFile resourceFile;

		private readonly IResourceManager resourceManager;

		private uint flags;

		private bool isShared;

		#endregion

		#region Constructors and Destructors

		public ResGroup(IResourceManager resourceManager, IResourceFile resourceFile, IComponentContext context)
		{
			this.resourceManager = resourceManager;
			this.resourceFile = resourceFile;
			this.context = context;

			//TODO: make public properties read-only
			this.externalResources = context.Resolve<IList<IResourceFile>>();
			this.embeddedResources = context.Resolve<IList<Managed>>();
		}

		#endregion

		#region Public Properties

		public override uint ClassHashCode
		{
			get
			{
				return TypeHash;
			}
		}

		public IList<Managed> EmbeddedResources
		{
			get
			{
				return this.embeddedResources;
			}
		}

		public IList<IResourceFile> ExternalResources
		{
			get
			{
				return this.externalResources;
			}
		}

		public uint Flags
		{
			get
			{
				return this.flags;
			}
			set
			{
				if (this.flags != value)
				{
					this.RaisePropertyChanging("Flags");
					this.flags = value;
					this.RaisePropertyChanged("Flags");
				}
			}
		}

		public bool IsShared
		{
			get
			{
				return this.isShared;
			}
			set
			{
				if (this.isShared != value)
				{
					this.RaisePropertyChanging("IsShared");
					this.isShared = value;
					this.RaisePropertyChanged("IsShared");
				}
			}
		}

		#endregion

		#region Public Methods and Operators

		public void AddFile(string fullPath)
		{
			var file = this.resourceManager.EnsureFile(fullPath);
			foreach (var f in this.externalResources)
			{
				if (f == file)
				{
					return;
				}
			}

			if (file != null)
			{
				this.externalResources.Add(file);

				file.Open();

				var extension = Path.GetExtension(file.FilePath);
				if (string.Compare(extension, ".geo", StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					var mtl = Path.ChangeExtension(file.FilePath, ".mtl");
					if (File.Exists(mtl))
					{
						this.AddFile(mtl);
					}
				}
			}
		}

		public void AddResource(Managed item)
		{
			if (this.embeddedResources.Contains(item))
			{
				throw new ApplicationException("item is in collection already");
			}

			this.embeddedResources.Add(item);
			this.SubscribeOnNameChange(item);
			//TODO: get source file reference
			this.resourceManager.ProvideResource(item.ClassHashCode, item.NameHash, item, this.resourceFile);
		}

		#endregion

		#region Methods

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				this.Clear();
			}
		}

		private void Clear()
		{
			while (this.externalResources.Count > 0)
			{
				this.RemoveFileAt(this.externalResources.Count - 1);
			}
			while (this.embeddedResources.Count > 0)
			{
				this.RemoveResourceAt(this.embeddedResources.Count - 1);
			}
		}

		private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "NameHash")
			{
				var item = ((Managed)sender);
				this.resourceManager.ProvideResource(item.ClassHashCode, item.NameHash, item, this.resourceFile);
			}
		}

		private void ItemPropertyChanging(object sender, PropertyChangingEventArgs e)
		{
			if (e.PropertyName == "NameHash")
			{
				var item = ((Managed)sender);
				this.resourceManager.RetractResource(item.ClassHashCode, item.NameHash, item, this.resourceFile);
			}
		}

		private void RemoveFileAt(int index)
		{
			var file = this.externalResources[index];
			this.externalResources.RemoveAt(index);
			file.Close();
		}

		private void RemoveResourceAt(int index)
		{
			var item = this.embeddedResources[index];
			this.embeddedResources.RemoveAt(index);

			this.UnsubscribeOnNameChange(item);

			this.resourceManager.RetractResource(item.ClassHashCode, item.NameHash, item, this.resourceFile);

			item.Dispose();
		}

		private void SubscribeOnNameChange(Managed item)
		{
			item.PropertyChanged += this.ItemPropertyChanged;
			item.PropertyChanging += this.ItemPropertyChanging;
		}

		private void UnsubscribeOnNameChange(Managed item)
		{
			item.PropertyChanged -= this.ItemPropertyChanged;
			item.PropertyChanging -= this.ItemPropertyChanging;
		}

		#endregion
	}
}