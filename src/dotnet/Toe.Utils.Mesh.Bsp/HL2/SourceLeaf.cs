namespace Toe.Utils.Mesh.Bsp.HL2
{
	public struct SourceLeaf
	{
		#region Constants and Fields

		public SourceCompressedLightCube ambientLighting; // Precaculated light info for entities.

		public short area_flags; // area this leaf is in

		public SourceBoundingBox box;

		public short cluster; // cluster this leaf is in

		public int contents; // OR of all brushes (not needed?)

		public ushort firstleafbrush; // index into leafbrushes

		public ushort firstleafface; // index into leaffaces

		public short leafWaterDataID; // -1 for not in water

		public ushort numleafbrushes;

		public ushort numleaffaces;

		public short padding; // padding to 4-byte boundary

		#endregion
	};
}