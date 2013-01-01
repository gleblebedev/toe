using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

using Autofac;

using Toe.Resources;
using Toe.Utils.Marmalade.IwGx;

namespace Toe.Utils.Marmalade
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
			if (f.EndsWith(".bmp"))
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
					Bitmap = LoadBitmap(filePath)
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

		private static Bitmap LoadBitmap(string filePath)
		{
			if (filePath.EndsWith(".tga", StringComparison.InvariantCultureIgnoreCase))
			{
				return LoadTGA(filePath);
			}
			return LoadDefault(filePath);
		}

		private static Bitmap LoadDefault(string filePath)
		{
			return (Bitmap)Image.FromFile(filePath);
		}

		private static Bitmap LoadTGA(string filePath)
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

		private static Bitmap LoadUncompressedTrueColorImage(BinaryReader fileStream, TgaHeader header)
		{
			if (header.depth == 24)
			{
				var b = new Bitmap(header.width, header.height, PixelFormat.Format24bppRgb);
				var BoundsRect = new Rectangle(0, 0, header.width, header.height);
				BitmapData bmpData = b.LockBits(BoundsRect, ImageLockMode.WriteOnly, b.PixelFormat);

				IntPtr ptr = bmpData.Scan0;
				int bytes = 3 * b.Width * b.Height;
				var rgbValues = new byte[bytes];
				int pos = 0;
				while (pos < rgbValues.Length)
				{
					var len = fileStream.Read(rgbValues, pos, rgbValues.Length - pos);
					if (len <= 0)
					{
						break;
					}
					pos += len;
				}
				var correctedRgbValues = new byte[bmpData.Stride * b.Height];
				for (int y = 0; y < b.Height; ++y)
				{
					int offset;
					if ((header.descriptor & 0x20) == 0)
						offset = (b.Height - 1 - y) * b.Width;
					else 
						offset = y * b.Width;
					for (int x = 0; x < b.Width; ++x)
					{
						int srcpos = 3 * (x + offset);
						var dstpos = x * 3 + y * bmpData.Stride;
						correctedRgbValues[dstpos + 0] = rgbValues[srcpos + 0];
						correctedRgbValues[dstpos + 1] = rgbValues[srcpos + 1];
						correctedRgbValues[dstpos + 2] = rgbValues[srcpos + 2];
					}
				}

				Marshal.Copy(correctedRgbValues, 0, ptr, correctedRgbValues.Length);

				b.UnlockBits(bmpData);
				return b;
			}
			else if (header.depth == 32)
			{
				var b = new Bitmap(header.width, header.height, PixelFormat.Format32bppArgb);
				var BoundsRect = new Rectangle(0, 0, header.width, header.height);
				BitmapData bmpData = b.LockBits(BoundsRect, ImageLockMode.WriteOnly, b.PixelFormat);

				IntPtr ptr = bmpData.Scan0;
				int bytes = bmpData.Stride * b.Height;
				var rgbValues = new byte[bytes];
				int pos = 0;
				while (pos < rgbValues.Length)
				{
					var len = fileStream.Read(rgbValues, pos, rgbValues.Length - pos);
					if (len <= 0)
					{
						break;
					}
					pos += len;
				}

				var correctedRgbValues = new byte[bmpData.Stride * b.Height];
				for (int y = 0; y < b.Height; ++y)
				{
					int offset;
					if ((header.descriptor & 0x20) == 0)
						offset = (b.Height - 1 - y) * b.Width;
					else
						offset = y * b.Width;
					for (int x = 0; x < b.Width; ++x)
					{
						int srcpos = 4 * (x + offset);
						var dstpos = x * 4 + y * bmpData.Stride;
						correctedRgbValues[dstpos + 0] = rgbValues[srcpos + 0];
						correctedRgbValues[dstpos + 1] = rgbValues[srcpos + 1];
						correctedRgbValues[dstpos + 2] = rgbValues[srcpos + 2];
						correctedRgbValues[dstpos + 3] = rgbValues[srcpos + 3];
					}
				}

				Marshal.Copy(rgbValues, 0, ptr, bytes);

				b.UnlockBits(bmpData);
				return b;
			}
			throw new FormatException();
		}

		#endregion
	}
}