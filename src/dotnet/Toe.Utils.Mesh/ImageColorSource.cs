using System.ComponentModel;

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


	}
}