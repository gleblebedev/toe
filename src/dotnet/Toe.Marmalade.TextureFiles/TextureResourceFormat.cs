using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

using Autofac;

using Toe.Marmalade.IwGx;
using Toe.Marmalade.TextFiles;
using Toe.Resources;
using Toe.Utils.Marmalade;

using Image = Toe.Marmalade.IwGx.Image;

namespace Toe.Marmalade.TextureFiles
{
	public class TextureResourceFormat : IResourceFileFormat
	{
		#region Constants and Fields

		private readonly IComponentContext context;

		private readonly IResourceManager resourceManager;

		#endregion

		#region Constructors and Destructors

		public TextureResourceFormat(IResourceManager resourceManager, IComponentContext context)
		{
			this.resourceManager = resourceManager;
			this.context = context;
		}

		#endregion

		#region Public Methods and Operators

		public bool CanRead(string filePath)
		{
			var f = filePath.ToLower();
			if (f.EndsWith(".bmp") || f.EndsWith(".png"))
			{
				return true;
			}
			if (f.EndsWith(".tga"))
			{
				return true;
			}
			return false;
		}

		public bool CanWrite(string filePath)
		{
			throw new NotImplementedException();
		}

		public IList<IResourceFileItem> Read(string filePath)
		{
			IList<IResourceFileItem> items = this.context.Resolve<IList<IResourceFileItem>>();

			var t = new Texture
				{
					BasePath = Path.GetDirectoryName(filePath),
					Name = Path.GetFileNameWithoutExtension(filePath),
					Image = LoadBitmap(filePath)
				};
			items.Add(new ResourceFileItem(Texture.TypeHash, t));
			return items;
		}

		public void Write(string filePath, IList<IResourceFileItem> items)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Methods

		private static Image LoadBitmap(string filePath)
		{
			if (filePath.EndsWith(".tga", StringComparison.InvariantCultureIgnoreCase))
			{
				return LoadTGA(filePath);
			}
			return LoadDefault(filePath);
		}

		private static Image LoadDefault(string filePath)
		{
			return new Image((Bitmap)System.Drawing.Image.FromFile(filePath));
		}

		private static Image LoadTGA(string filePath)
		{
			using (var fileStream = new BinaryReader(File.OpenRead(filePath)))
			{
				TgaHeader header = new TgaHeader();
				header.imageIDLength = fileStream.ReadByte();
				header.colorMapType = fileStream.ReadByte();
				header.imageType = fileStream.ReadByte();

				header.firstEntryIndex = fileStream.ReadUInt16();
				header.colorMapLength = fileStream.ReadUInt16();
				header.colorMapEntrySize = fileStream.ReadByte();

				header.xOrigin = fileStream.ReadUInt16();
				//absolute coordinate of lower-left corner for displays where origin is at the lower left
				header.yOrigin = fileStream.ReadUInt16(); //as for X-origin
				header.width = fileStream.ReadUInt16();
				header.height = fileStream.ReadUInt16();
				header.depth = fileStream.ReadByte();
				header.descriptor = fileStream.ReadByte();

				if (header.colorMapType != 0)
				{
					throw new FormatException("image file contains color map");
				}
				if (header.imageType == 2)
				{
					return LoadUncompressedTrueColorImage(fileStream, header);
				}
				throw new FormatException("Only uncompressed true-color image supported");
			}
		}

		private static Image LoadUncompressedTrueColorImage(BinaryReader fileStream, TgaHeader header)
		{
			if (header.depth == 24)
			{
				var img = new Image();
				img.width = header.width;
				img.height = header.height;
				img.pitch = (ushort)(img.width * 3);
				img.Format = ImageFormat.RGB_888;
				img.data = new byte[img.height * img.pitch];

				int pos = 0;
				while (pos < img.data.Length)
				{
					var len = fileStream.Read(img.data, pos, img.data.Length - pos);
					if (len <= 0)
					{
						break;
					}
					pos += len;
				}
				if ((header.descriptor & 0x20) == 0)
				{
					img.FlipVerticaly();
				}

				return img;
			}
			else if (header.depth == 32)
			{
				var img = new Image();
				img.width = header.width;
				img.height = header.height;
				img.pitch = (ushort)(img.width * 4);
				img.Format = ImageFormat.ABGR_8888;
				img.data = new byte[img.height * img.pitch];

				int pos = 0;
				while (pos < img.data.Length)
				{
					var len = fileStream.Read(img.data, pos, img.data.Length - pos);
					if (len <= 0)
					{
						break;
					}
					pos += len;
				}
				if ((header.descriptor & 0x20) == 0)
				{
					img.FlipVerticaly();
				}
				for (int i = 0; i< img.data.Length; i+=4)
				{
					byte b = img.data[i];
					img.data[i ] = img.data[i+2];
					img.data[i + 2] = b;
				}

					return img;
			}
			throw new FormatException();
		}

		#endregion
	}
}