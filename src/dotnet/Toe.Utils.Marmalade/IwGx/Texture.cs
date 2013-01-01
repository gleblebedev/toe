using System.Drawing;
using System.Drawing.Imaging;

using OpenTK.Graphics.OpenGL;

using Toe.Gx;
using Toe.Resources;

using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace Toe.Utils.Marmalade.IwGx
{
	public class Texture : Managed
	{
		#region Constants and Fields

		public static readonly uint TypeHash = Hash.Get("CIwTexture");

		private Bitmap bitmap;

		private uint textureId;

		#endregion

		#region Public Properties

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

		#endregion

		#region Public Methods and Operators

		public void ApplyOpenGL(int stage)
		{
			GL.ActiveTexture(TextureUnit.Texture0 + stage);
			OpenTKHelper.Assert();

			if (this.textureId == 0)
			{
				GL.GenTextures(1, out this.textureId);
				OpenTKHelper.Assert();
				GL.PushAttrib(AttribMask.TextureBit);
				try
				{
					GL.BindTexture(TextureTarget.Texture2D, this.textureId);
					OpenTKHelper.Assert();
					BitmapData data = this.bitmap.LockBits(
						new Rectangle(0, 0, this.bitmap.Width, this.bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
					GL.TexImage2D(
						TextureTarget.Texture2D,
						0,
						PixelInternalFormat.Rgba,
						data.Width,
						data.Height,
						0,
						OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
						PixelType.UnsignedByte,
						data.Scan0);
					GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
					GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
					OpenTKHelper.Assert();
					this.bitmap.UnlockBits(data);
					GL.Finish();
					OpenTKHelper.Assert();
				}
				finally
				{
					GL.PopAttrib();
				}
			}
			GL.Enable(EnableCap.Texture2D);
			OpenTKHelper.Assert();
			GL.BindTexture(TextureTarget.Texture2D, this.textureId);
			OpenTKHelper.Assert();
		}

		public override uint ClassHashCode
		{
			get
			{
				return TypeHash;
			}
		}

		#endregion

		#region Methods

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				if (this.bitmap != null)
				{
					this.bitmap.Dispose();
				}
			}
		}

		#endregion
	}
}