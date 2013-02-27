using System;
using System.Drawing;

namespace Toe.Utils.Mesh
{
	public class SolidColorSource : ClassWithNotification, IColorSource
	{
		#region Constants and Fields

		protected static PropertyEventArgs ColorEventArgs = Expr.PropertyEventArgs<SolidColorSource>(x => x.Color);

		private Color color;

		#endregion

		#region Public Properties

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

		public bool IsColor
		{
			get
			{
				return true;
			}
		}

		public bool IsImage
		{
			get
			{
				return false;
			}
		}

		public ColorSourceType Type
		{
			get
			{
				return ColorSourceType.SolidColor;
			}
		}

		#endregion

		#region Public Methods and Operators

		public Color GetColor()
		{
			return this.color;
		}

		public string GetImagePath()
		{
			throw new InvalidOperationException();
		}

		public byte[] GetImageRawData()
		{
			return new byte[4] { this.color.R, this.color.G, this.color.B, this.color.A };
		}

		#endregion
	}
}