using System.Drawing;

namespace Toe.Utils.Mesh
{
	public class SolidColorSource : ClassWithNotification, IColorSource
	{
		#region Implementation of IColorSource

		public ColorSourceType Type
		{
			get
			{
				return ColorSourceType.SolidColor;
			}
		}

		public bool IsColor
		{
			get
			{
				return true;
			}
		}

		protected static PropertyEventArgs ColorEventArgs = Expr.PropertyEventArgs<SolidColorSource>(x => x.Color);

		private Color color;

		public Color Color
		{
			get
			{
				return this.color;
			}
			set
			{
				if (this.color != value)
				{
					this.RaisePropertyChanging(ColorEventArgs.Changing);
					this.color = value;
					this.RaisePropertyChanged(ColorEventArgs.Changed);
				}
			}
		}

		public Color GetColor()
		{
			return color;
		}

		public bool IsImage
		{
			get
			{
				return false;
			}
		}

		public string GetImagePath()
		{
			throw new System.InvalidOperationException();
		}

		public byte[] GetImageRawData()
		{
			return new byte[4] { color.R, color.G, color.B, color.A };
		}

		#endregion
	}
}