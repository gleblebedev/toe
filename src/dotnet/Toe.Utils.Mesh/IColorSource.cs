using System.Drawing;

namespace Toe.Utils.Mesh
{
	public interface IColorSource
	{
		ColorSourceType Type { get; }

		bool IsColor { get; }
		Color GetColor();

		bool IsImage { get; }
		string GetImagePath();
		byte[] GetImageRawData();

	}
}