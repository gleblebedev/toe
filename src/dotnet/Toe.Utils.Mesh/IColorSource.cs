using System.Drawing;

namespace Toe.Utils.Mesh
{
	public interface IColorSource
	{
		#region Public Properties

		bool IsColor { get; }

		bool IsImage { get; }

		ColorSourceType Type { get; }

		#endregion

		#region Public Methods and Operators

		Color GetColor();

		string GetImagePath();

		byte[] GetImageRawData();

		#endregion
	}
}