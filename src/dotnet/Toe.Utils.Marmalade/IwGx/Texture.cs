using System.Drawing;
using System.Drawing.Imaging;

using OpenTK.Graphics.OpenGL;

using Toe.Resources;

using All = OpenTK.Graphics.OpenGL.All;
using GL = OpenTK.Graphics.OpenGL.GL;
using PixelType = OpenTK.Graphics.OpenGL.PixelType;

namespace Toe.Utils.Mesh.Marmalade.IwGx
{
	public class Texture : Managed
	{
		public static readonly uint TypeHash = Hash.Get("CIwTexture");

		private Bitmap bitmap;

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				if (bitmap != null) bitmap.Dispose();
			}
		}

		public Bitmap Bitmap
		{
			get
			{
				return this.bitmap;
			}
			set
			{
				if (this.bitmap != value)
				{
					this.bitmap = value;
					this.RaisePropertyChanged("Bitmap");
				}
			}
		}

		#region Overrides of Managed

		public override uint GetClassHashCode()
		{
			return TypeHash;
		}

		#endregion

		public void ApplyOpenGL(int stage)
		{
			if (textureId == 0)
			{
				GL.GenTextures(1, out textureId);
				OpenTKHelper.Assert();
				GL.BindTexture(TextureTarget.Texture2D, textureId);
				OpenTKHelper.Assert();
				BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
				GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
				OpenTKHelper.Assert();
				bitmap.UnlockBits(data);
				GL.Finish();
				OpenTKHelper.Assert();
			}
			GL.ActiveTexture(TextureUnit.Texture0 + stage);
			OpenTKHelper.Assert();
			GL.Enable(EnableCap.Texture2D);
			OpenTKHelper.Assert();
			GL.BindTexture(TextureTarget.Texture2D, textureId);
			OpenTKHelper.Assert();
		}

		private uint textureId = 0;
	}
}