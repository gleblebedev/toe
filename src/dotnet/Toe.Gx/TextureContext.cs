using System;

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using Toe.Marmalade;
using Toe.Marmalade.IwGx;

namespace Toe.Gx
{
	public class TextureContext : IContextData, IDisposable
	{
		#region Constants and Fields

		private readonly Texture texture;

		private IGraphicsContext context;

		private uint textureId;

		#endregion

		#region Constructors and Destructors

		public TextureContext(Texture texture)
		{
			this.texture = texture;
		}

		~TextureContext()
		{
			this.Dispose(false);
		}

		#endregion

		#region Public Methods and Operators

		public void ApplyToChannel(int stage)
		{
			GL.ActiveTexture(TextureUnit.Texture0 + stage);
			OpenTKHelper.Assert();

			if (this.textureId == 0)
			{
				this.GenTexture();
			}
			else if (this.context.IsDisposed)
			{
				this.GenTexture();
			}
			GL.Enable(EnableCap.Texture2D);
			OpenTKHelper.Assert();
			GL.BindTexture(TextureTarget.Texture2D, this.textureId);
			OpenTKHelper.Assert();
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			this.Dispose(true);
		}

		public void UploadImage(Image image)
		{
			PixelFormat pixelFormat;
			PixelInternalFormat pixelInternalFormat = PixelInternalFormat.Rgba;
			switch (image.Format)
			{
				case ImageFormat.RGB_888:
					pixelFormat = PixelFormat.Bgr;
					break;
				case ImageFormat.BGR_888:
					pixelFormat = PixelFormat.Rgb;
					break;
				case ImageFormat.ABGR_8888:
					pixelFormat = PixelFormat.Rgba;
					break;
				default:
					{
						this.UploadImage(image.ConvertToAbgr8888());
						return;
					}
			}
			GL.TexImage2D(
				TextureTarget.Texture2D,
				0,
				pixelInternalFormat,
				image.width,
				image.height,
				0,
				pixelFormat,
				PixelType.UnsignedByte,
				image.data);
			OpenTKHelper.Assert();
		}

		#endregion

		#region Methods

		private void Dispose(bool disposing)
		{
		}

		private void GenTexture()
		{
			this.context = GraphicsContext.CurrentContext;

			GL.GenTextures(1, out this.textureId);
			OpenTKHelper.Assert();
			GL.PushAttrib(AttribMask.TextureBit);
			try
			{
				GL.BindTexture(TextureTarget.Texture2D, this.textureId);
				OpenTKHelper.Assert();
				this.UploadImage(this.texture.Image);

				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
				OpenTKHelper.Assert();
				GL.Finish();
				OpenTKHelper.Assert();
			}
			finally
			{
				GL.PopAttrib();
			}
		}

		#endregion
	}
}