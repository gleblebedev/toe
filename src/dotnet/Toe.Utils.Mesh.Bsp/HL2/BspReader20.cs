using System.IO;

namespace Toe.Utils.Mesh.Bsp.HL2
{
	public class BspReader20 : BspReader19, IBspReader
	{
		protected override void ReadLeaves()
		{
			this.ReadLeaves(32);
		}

		protected override void ReadLeaf(ref SourceLeaf sourceLeaf)
		{
			sourceLeaf.contents = Stream.ReadInt32();               // OR of all brushes (not needed?)
			sourceLeaf.cluster = Stream.ReadInt16();               // cluster this leaf is in
			sourceLeaf.area_flags = Stream.ReadInt16();                 // area this leaf is in
			this.ReadBBox(ref sourceLeaf.box);
			sourceLeaf.firstleafface = Stream.ReadUInt16();          // index into leaffaces
			sourceLeaf.numleaffaces = Stream.ReadUInt16();
			sourceLeaf.firstleafbrush = Stream.ReadUInt16();         // index into leafbrushes
			sourceLeaf.numleafbrushes = Stream.ReadUInt16();
			sourceLeaf.leafWaterDataID = Stream.ReadInt16();        // -1 for not in water
			sourceLeaf.padding = Stream.ReadInt16();                // padding to 4-byte boundary
		}
	}
}
