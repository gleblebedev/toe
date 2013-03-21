using OpenTK;

using Toe.Utils;

namespace Toe.Marmalade.IwGx
{
	public class Texture : Managed
	{
		#region Constants and Fields

		public static readonly uint TypeHash = Hash.Get("CIwTexture");

		public Vector2 UVScale = new Vector2(1, 1);

		protected static PropertyEventArgs FlagsEventArgs = Expr.PropertyEventArgs<Texture>(x => x.Flags);

		protected static PropertyEventArgs FormatHWEventArgs = Expr.PropertyEventArgs<Texture>(x => x.FormatHW);

		protected static PropertyEventArgs FormatSWEventArgs = Expr.PropertyEventArgs<Texture>(x => x.FormatSW);

		protected static PropertyEventArgs ImageEventArgs = Expr.PropertyEventArgs<Texture>(x => x.Image);

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
					this.RaisePropertyChanging(FlagsEventArgs.Changing);
					this.flags = value;
					this.RaisePropertyChanged(FlagsEventArgs.Changed);
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
					this.RaisePropertyChanging(FormatHWEventArgs.Changing);
					this.formatHw = value;
					this.RaisePropertyChanged(FormatHWEventArgs.Changed);
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
					this.RaisePropertyChanging(FormatSWEventArgs.Changing);
					this.formatSw = value;
					this.RaisePropertyChanged(FormatSWEventArgs.Changed);
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
					this.RaisePropertyChanging(ImageEventArgs.Changing);
					this.image = value;
					this.RaisePropertyChanged(ImageEventArgs.Changed);
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