using System;
using System.Drawing;

namespace Toe.Utils.Mesh
{
	public class FunctionColorSource : IColorSource
	{
		#region Public Properties

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
				return false;
			}
		}

		public ColorSourceType Type
		{
			get
			{
				return ColorSourceType.Function;
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
			throw new InvalidOperationException();
		}

		public byte[] GetImageRawData()
		{
			//TODO: render function into image
			throw new NotImplementedException();
		}

		#endregion
	}
}