using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Autofac;
using Autofac.Core.Activators.Reflection;

namespace Toe.Resources
{
	public class ResourceManager : IResourceManager
	{
		~ResourceManager()
		{
			this.Dispose(false);
		}

		protected virtual void Dispose(bool disposing)
		{
			
		}

		private readonly IComponentContext context;

		public ResourceManager(IComponentContext context)
		{
			this.context = context;
		}

		readonly Dictionary<string,IResourceFile> files = new Dictionary<string, IResourceFile>();

		readonly Dictionary<uint, Dictionary<uint, IResourceItem>> resources = new Dictionary<uint, Dictionary<uint, IResourceItem>>();

		public IResourceFile EnsureFile(string filePath)
		{
			var fullPath = Path.GetFullPath(filePath);
			var key = fullPath.ToLower();
			IResourceFile resourceFile;
			if (files.TryGetValue(key, out resourceFile))
			{
				return resourceFile;
			}
			resourceFile = context.Resolve<IResourceFile>(new[] { TypedParameter.From<IResourceManager>(this), TypedParameter.From(fullPath) });
			files.Add(key, resourceFile);

			return resourceFile;
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
		public IResourceItem EnsureItem(uint type, uint hash)
		{
			return EnsureItem(this.EnsureTypeCollection(type),type, hash);
		}
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

		public IResourceItem ConsumeResource(uint type, uint nameHash)
		{
			var consumeResource = (ResourceItem)EnsureItem(type, nameHash);
			consumeResource.Consume();
			return consumeResource;
		}

		public void ReleaseResource(uint type, uint nameHash)
		{
			var consumeResource = (ResourceItem)EnsureItem(type, nameHash);
			consumeResource.Release();
		}

		public void ProvideResource(uint type, uint nameHash, object item, IResourceFile source)
		{
			var consumeResource = (ResourceItem)EnsureItem(type, nameHash);
			consumeResource.Provide(item,source);
		}

		public void RetractResource(uint type, uint nameHash, object item, IResourceFile source)
		{
			var consumeResource = (ResourceItem)EnsureItem(type, nameHash);
			consumeResource.Retract(item,source);
			TryToRemoveResource(consumeResource);
		}

		private void TryToRemoveResource(ResourceItem resourceItem)
		{
			if (!resourceItem.IsInUse)
			{
				var typeCollection = this.EnsureTypeCollection(resourceItem.Type);
				typeCollection.Remove(resourceItem.Hash);
			}
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

		#region Implementation of IDisposable

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion
	}
}