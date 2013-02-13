using System.Collections.Generic;

namespace Toe.Utils.Mesh.Bsp.HL2
{
	public struct SourceLeaf
	{
		public int contents;               // OR of all brushes (not needed?)
		public short cluster;                // cluster this leaf is in
		public short area_flags;                 // area this leaf is in
		public SourceBoundingBox box;

		public ushort firstleafface;          // index into leaffaces
		public ushort numleaffaces;
		public ushort firstleafbrush;         // index into leafbrushes
		public ushort numleafbrushes;
		public short leafWaterDataID;        // -1 for not in water
		public SourceCompressedLightCube ambientLighting;  // Precaculated light info for entities.
		public short padding;                // padding to 4-byte boundary
	};
}