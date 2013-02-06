using OpenTK.Graphics.OpenGL;

using Toe.Utils.Mesh;

namespace Toe.Gx
{
	//[StructLayout(LayoutKind.Sequential)]
	public struct ToeGxIndexBufferItem
	{
		public object Material;

		public BeginMode Mode;

		public int Size;
	}
}