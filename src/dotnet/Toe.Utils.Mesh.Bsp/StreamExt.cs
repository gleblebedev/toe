using System.IO;

namespace Toe.Utils.Mesh.Bsp
{
	public static class StreamExt
	{
		public static uint ReadUInt32(this Stream stream)
		{
			var a0 = (uint)stream.ReadByte();
			var a1 = (uint)stream.ReadByte();
			var a2 = (uint)stream.ReadByte();
			var a3 = (uint)stream.ReadByte();
			return ((a3 << 24) | (a2 << 16) | (a1 << 8) | (a0));
		}
		public static int ReadInt32(this Stream stream)
		{
			return (int)stream.ReadUInt32();
		}
	}
}