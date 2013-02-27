using System.Drawing;

namespace Toe.Utils.Mesh
{
	public class ImageColorSource : ClassWithNotification, IColorSource
	{
		#region Constants and Fields

		private static PropertyEventArgs ImageEventArgs = Expr.PropertyEventArgs<ImageColorSource>(x => x.Image);

		private IImage image;

		#endregion

		#region Public Properties

		public IImage Image
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

		public bool IsColor
		{
			get
			{
				return false;
			}
		}

		public bool IsImage
		{
			get
			{
				return true;
			}
		}

		public ColorSourceType Type
		{
			get
			{
				return ColorSourceType.Image;
			}
		}

		#endregion

		#region Public Methods and Operators

		public Color GetColor()
		{
			return Color.White;
		}

		public string GetImagePath()
		{
			return this.image.GetFilePath();
		}

		public byte[] GetImageRawData()
		{
			return this.image.GetRawData();
		}

		#endregion
	}
}