using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using Toe.Resources;
using Toe.Utils;

namespace Toe.Marmalade.IwGx
{
	public class Texture : Managed
	{
		#region Constants and Fields

		public static readonly uint TypeHash = Hash.Get("CIwTexture");

		public Vector2 UVScale;

		private uint flags;

		private ImageFormat formatHw = ImageFormat.FORMAT_UNDEFINED;

		private ImageFormat formatSw = ImageFormat.FORMAT_UNDEFINED;

		private Image image;

		#endregion

		#region Public Properties

		public override uint ClassHashCode
		{
			get
			{
				return TypeHash;
			}
		}

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

		public ImageFormat FormatHW
		{
			get
			{
				return this.formatHw;
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

		public ImageFormat FormatSW
		{
			get
			{
				return this.formatSw;
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

		#endregion


		#region Methods

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
			}
		}

		

		#endregion
	}
}