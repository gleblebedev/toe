using System.ComponentModel;
using System.Drawing;

namespace Toe.Utils.Mesh
{
	public class ImageColorSource : ClassWithNotification, IColorSource
	{
		#region Implementation of IColorSource

		public ColorSourceType Type
		{
			get
			{
				return ColorSourceType.Image;
			}
		}

		public bool IsColor
		{
			get
			{
				return false;
			}
		}

		#endregion

		private static PropertyEventArgs ImageEventArgs = Expr.PropertyEventArgs<ImageColorSource>(x => x.Image);

		private IImage image;

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

		public Color GetColor()
		{
			return Color.White;
		}

		public bool IsImage
		{
			get
			{
				return true;
			}
		}

		public string GetImagePath()
		{
			return this.image.GetFilePath();
		}

		public byte[] GetImageRawData()
		{
			return this.image.GetRawData();
		}
	}
}