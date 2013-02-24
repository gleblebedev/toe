using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Toe.Utils.Mesh
{
	public class EmbeddedImage : SceneItem, IImage
	{
		private int width;

		private int height;

		private int pitch;

		private byte[] data;

		public EmbeddedImage()
		{
			
		}
		
		public EmbeddedImage(Bitmap bitmap)
		{
			this.width = bitmap.Width;
			this.height = bitmap.Height;
			this.pitch = this.Width * 4;
			BitmapData data = bitmap.LockBits(
				new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			this.data = new byte[this.Pitch * this.Height];
			Marshal.Copy(data.Scan0, this.data, 0, this.data.Length);
			for (int i = 0; i < this.data.Length; i += 4)
			{
				byte b = this.data[i];
				this.data[i] = this.data[i + 2];
				this.data[i + 2] = b;
			}
			bitmap.UnlockBits(data);
		}

		public int Width
		{
			get
			{
				return this.width;
			}
			
		}

		public int Height
		{
			get
			{
				return this.height;
			}
			
		}

		public int Pitch
		{
			get
			{
				return this.pitch;
			}
			
		}

		public string GetFilePath()
		{
			return null;
		}

		public byte[] GetRawData()
		{
			return data;
		}
	}
}