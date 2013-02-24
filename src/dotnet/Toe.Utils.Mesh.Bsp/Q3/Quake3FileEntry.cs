using System.IO;

namespace Toe.Utils.Mesh.Bsp.Q3
{
	public struct Quake3FileEntry
	{
		#region Constants and Fields

		public uint offset;

		public uint size;

		#endregion

		#region Methods

		internal void Read(BinaryReader source)
		{
			this.offset = source.ReadUInt32();
			this.size = source.ReadUInt32();
		}

		#endregion
	}
}