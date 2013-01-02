using System.Drawing;
using System.Drawing.Imaging;

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using Toe.Gx;
using Toe.Resources;
using Toe.Utils.Marmalade;

using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace Toe.Marmalade.IwGx
{
	public class Texture : Managed
	{
		#region Constants and Fields

		public static readonly uint TypeHash = Hash.Get("CIwTexture");

		//private Bitmap bitmap;

		private uint textureId;

		private IGraphicsContext context;

		#endregion

		#region Public Properties

		//public Bitmap Bitmap
		//{
		//    get
		//    {
		//        return this.bitmap;
		//    }
		//    set
		//    {
		//        if (this.bitmap != value)
		//        {
		//            this.bitmap = value;
		//            this.RaisePropertyChanged("Bitmap");
		//        }
		//    }
		//}

		#endregion

		#region Public Methods and Operators

		public void ApplyOpenGL(int stage)
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
				Image.OpenGLUpload();
				

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

		public override uint ClassHashCode
		{
			get
			{
				return TypeHash;
			}
		}

		private uint flags;

		private ImageFormat formatSw = ImageFormat.FORMAT_UNDEFINED;

		private ImageFormat formatHw = ImageFormat.FORMAT_UNDEFINED;

		public uint Flags
		{
			get
			{
				return this.flags;
			}
			set
			{
				if (this.flags != value)
				{
					this.flags = value;
					this.RaisePropertyChanged("Flags");
				}
			}
		}

		public ImageFormat FormatSW
		{
			get
			{
				return formatSw;
			}
			set
			{
				if (this.formatSw != value)
				{
					this.formatSw = value;
					this.RaisePropertyChanged("FormatSW");
				}
			}
		}
		public ImageFormat FormatHW
		{
			get
			{
				return formatHw;
			}
			set
			{
				if (this.formatHw != value)
				{
					this.formatHw = value;
					this.RaisePropertyChanged("FormatHW");
				}
			}
		}
		#endregion

		#region Methods

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				//if (this.bitmap != null)
				//{
				//    this.bitmap.Dispose();
				//}
			}
		}

		#endregion

		private Image image;

		public Image Image
		{
			get
			{
				return this.image;
			}
			set
			{
				if (this.image != value)
				{
					this.image = value;
					this.RaisePropertyChanged("Image");
				}				
			}
		}
	}
}