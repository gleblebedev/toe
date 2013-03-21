using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Autofac;
using Autofac.Core;

namespace Toe.Resources
{
	public class ResourceManager : IResourceManager
	{
		#region Constants and Fields

		private readonly IComponentContext context;

		private readonly Dictionary<string, IResourceFile> files = new Dictionary<string, IResourceFile>();

		private readonly Dictionary<uint, Dictionary<uint, IResourceItem>> resources =
			new Dictionary<uint, Dictionary<uint, IResourceItem>>();

		#endregion

		#region Constructors and Destructors

		public ResourceManager(IComponentContext context)
		{
			this.context = context;
		}

		~ResourceManager()
		{
			this.Dispose(false);
		}

		#endregion

		#region Public Methods and Operators

		public IResourceItem ConsumeResource(uint type, uint nameHash)
		{
			var consumeResource = (ResourceItem)this.EnsureItem(type, nameHash);
			consumeResource.Consume();
			return consumeResource;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		public IResourceFile EnsureFile(string filePath)
		{
			var fullPath = Path.GetFullPath(filePath);
			var key = fullPath.ToLower();
			IResourceFile resourceFile;
			if (this.files.TryGetValue(key, out resourceFile))
			{
				return resourceFile;
			}

			resourceFile =
				this.context.Resolve<IResourceFile>(
					new Parameter[] { TypedParameter.From<IResourceManager>(this), TypedParameter.From(fullPath) });
			this.files.Add(key, resourceFile);
			//WatchFile(fullPath, resourceFile);

			return resourceFile;
		}

		public IResourceItem EnsureItem(uint type, uint hash)
		{
			return this.EnsureItem(this.EnsureTypeCollection(type), type, hash);
		}

		public Dictionary<uint, IResourceItem> EnsureTypeCollection(uint type)
		{
			Dictionary<uint, IResourceItem> typeCollection;
			if (!this.resources.TryGetValue(type, out typeCollection))
			{
				typeCollection = new Dictionary<uint, IResourceItem>();
				this.resources.Add(type, typeCollection);
			}
			return typeCollection;
		}

		public object FindResource(uint type, uint hash)
		{
			var typeCollection = this.EnsureTypeCollection(type);
			IResourceItem item;
			if (!typeCollection.TryGetValue(hash, out item))
			{
				return null;
			}
			return item.Value;
		}

		public IList<IResourceItem> GetAllResourcesOfType(uint type)
		{
			var typeCollection = this.EnsureTypeCollection(type);
			return (from i in typeCollection.Values select i).ToList();
		}

		public void ProvideResource(uint type, uint nameHash, object item, IResourceFile source)
		{
			var consumeResource = (ResourceItem)this.EnsureItem(type, nameHash);
			consumeResource.Provide(item, source);
		}

		public void ReleaseResource(uint type, uint nameHash)
		{
			var consumeResource = (ResourceItem)this.EnsureItem(type, nameHash);
			consumeResource.Release();
		}

		public void RetractResource(uint type, uint nameHash, object item, IResourceFile source)
		{
			var consumeResource = (ResourceItem)this.EnsureItem(type, nameHash);
			consumeResource.Retract(item, source);
			this.TryToRemoveResource(consumeResource);
		}

		#endregion

		#region Methods

		internal IResourceItem EnsureItem(Dictionary<uint, IResourceItem> typeCollection, uint type, uint hash)
		{
			IResourceItem item;
			if (!typeCollection.TryGetValue(hash, out item))
			{
				item = new ResourceItem(this, type, hash);
				typeCollection.Add(hash, item);
			}
			return item;
		}

		protected virtual void Dispose(bool disposing)
		{
		}

		private void TryToRemoveResource(ResourceItem resourceItem)
		{
			if (!resourceItem.IsInUse)
			{
				var typeCollection = this.EnsureTypeCollection(resourceItem.Type);
				typeCollection.Remove(resourceItem.Hash);
			}
		}

		#endregion
	}
}