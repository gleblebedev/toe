using System.Drawing;

namespace Toe.Utils.Mesh
{
	public class FunctionColorSource : IColorSource
	{
		#region Implementation of IColorSource

		public ColorSourceType Type
		{
			get
			{
				return ColorSourceType.Function;
			}
		}

		public bool IsColor
		{
			get
			{
				return false;
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
				return false;
			}
		}

		public string GetImagePath()
		{
			throw new System.InvalidOperationException();
		}

		public byte[] GetImageRawData()
		{
			//TODO: render function into image
			throw new System.NotImplementedException();
		}

		#endregion
	}
}