namespace Toe.Utils.Mesh.Bsp.Q3
{
	public struct Quake3FileEntry
	{
		public uint offset;
		public uint size;

		internal void Read(System.IO.BinaryReader source)
		{
			this.offset = source.ReadUInt32();
			this.size = source.ReadUInt32();
		}
	}
}