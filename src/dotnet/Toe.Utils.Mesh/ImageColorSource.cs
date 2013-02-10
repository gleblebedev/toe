using System.ComponentModel;

namespace Toe.Utils.Mesh
{
	public class ImageColorSource : IColorSource, INotifyPropertyChanged
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

		protected static PropertyChangedEventArgs ImageChangedEventArgs = new PropertyChangedEventArgs(Expr.Path<ImageColorSource>(a => a.Image));

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
					this.image = value;
					RaisePropertyChanged(ImageChangedEventArgs);
				}
			}
		}

		private void RaisePropertyChanged(PropertyChangedEventArgs property)
		{
			if (PropertyChanged != null) PropertyChanged(this, property);
		}

		#region Implementation of INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}
}