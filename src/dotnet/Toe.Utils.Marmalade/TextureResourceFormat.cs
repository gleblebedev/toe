using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

using Autofac;

using Toe.Resources;
using Toe.Utils.Mesh.Marmalade.IwGx;

namespace Toe.Utils.Mesh.Marmalade
{
	public class TextureResourceFormat: IResourceFileFormat
	{
		private readonly IResourceManager resourceManager;

		private readonly IComponentContext context;

		public TextureResourceFormat(IResourceManager resourceManager, IComponentContext context)
		{
			this.resourceManager = resourceManager;
			this.context = context;
		}

		#region Implementation of IResourceFileFormat

		public bool CanRead(string filePath)
		{
			var f = filePath.ToLower();
			if (f.EndsWith(".bmp")) return true;
			return false;
		}

		public IList<IResourceFileItem> Read(string filePath)
		{
			IList<IResourceFileItem> items = this.context.Resolve<IList<IResourceFileItem>>();
			var t = new Texture { BasePath = Path.GetDirectoryName(filePath), Name = Path.GetFileNameWithoutExtension(filePath), Bitmap = (Bitmap)Bitmap.FromFile(filePath) };
			items.Add(new ResourceFileItem(Texture.TypeHash, t));
			return items;
		}

		public bool CanWrite(string filePath)
		{
			throw new System.NotImplementedException();
		}

		public void Write(string filePath, IList<IResourceFileItem> items)
		{
			throw new System.NotImplementedException();
		}

		#endregion
	}
}