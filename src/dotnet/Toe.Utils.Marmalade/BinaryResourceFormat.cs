using System;
using System.Collections.Generic;
using System.IO;

using Autofac;

using Toe.Resources;

namespace Toe.Utils.Mesh.Marmalade
{
	public class BinaryResourceFormat : IResourceFileFormat
	{
		#region Constants and Fields

		private readonly IComponentContext context;

		private readonly IResourceManager resourceManager;

		#endregion

		#region Constructors and Destructors

		public BinaryResourceFormat(IResourceManager resourceManager, IComponentContext context)
		{
			this.resourceManager = resourceManager;
			this.context = context;
		}

		#endregion

		#region Public Methods and Operators

		public bool CanRead(string filePath)
		{
			var f = filePath.ToLower();
			if (f.EndsWith(".group.bin"))
			{
				return true;
			}
			return false;
		}

		public bool CanWrite(string filePath)
		{
			return false;
		}

		public IList<Managed> Load(Stream stream, string basePath)
		{
			IList<Managed> items = this.context.Resolve<IList<Managed>>();
			using (var source = new BinaryReader(stream))
			{
				var parser = new BinaryParser(source, basePath);
			}
			return items;
		}

		public IList<IResourceFileItem> Read(string filePath)
		{
			var items = this.context.Resolve<IList<IResourceFileItem>>();

			using (var fileStream = File.OpenRead(filePath))
			{
				var resources = this.Load(fileStream, Path.GetDirectoryName(Path.GetFullPath(filePath)));
				foreach (var resource in resources)
				{
					items.Add(new ResourceFileItem(resource.GetClassHashCode(), resource));
				}
			}

			return items;
		}

		public void Write(string filePath, IList<IResourceFileItem> items)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}