using System.IO;

namespace Toe.Utils.Mesh.Bsp.HL2
{
	public class BspReader19 : BspReader17, IBspReader
	{
		protected override void ReadFaces()
		{
			SeekEntryAt(header.Faces.offset);
			int size = EvalNumItems(header.Faces.size, 56);
			faces = new SourceFace[size];
			for (int i = 0; i < size; ++i)
			{
				this.ReadFace(ref this.faces[i]);
			}
			AssertStreamPossition(header.Faces.size + header.Faces.offset);
		}

		protected override void ReadLeaves()
		{
			this.ReadLeaves(56);
		}

		protected override void ReadLeaf(ref SourceLeaf sourceLeaf)
		{
			sourceLeaf.contents = Stream.ReadInt32();               // OR of all brushes (not needed?)
			sourceLeaf.cluster = Stream.ReadInt16();                // cluster this leaf is in
			sourceLeaf.area_flags = Stream.ReadInt16();                 // area this leaf is in
			this.ReadBBox(ref sourceLeaf.box);
			sourceLeaf.firstleafface = Stream.ReadUInt16();          // index into leaffaces
			sourceLeaf.numleaffaces = Stream.ReadUInt16();
			sourceLeaf.firstleafbrush = Stream.ReadUInt16();         // index into leafbrushes
			sourceLeaf.numleafbrushes = Stream.ReadUInt16();
			sourceLeaf.leafWaterDataID = Stream.ReadInt16();        // -1 for not in water
			this.ReadCompressedLightCube(ref sourceLeaf.ambientLighting);
			sourceLeaf.padding = Stream.ReadInt16();                // padding to 4-byte boundary
		}


		protected override void ReadFace(ref SourceFace face)
		{
			face.planenum = Stream.ReadUInt16();		// the plane number
			face.side = (byte)Stream.ReadByte();			// faces opposite to the node's plane direction
			face.onNode = (byte)Stream.ReadByte(); 			// 1 of on node, 0 if in leaf
			face.firstedge = Stream.ReadInt32();			// index into surfedges	
			face.numedges = Stream.ReadInt16();			// number of surfedges
			face.texinfo = Stream.ReadInt16();			// texture info
			face.dispinfo = Stream.ReadInt16();			// displacement info
			face.surfaceFogVolumeID = Stream.ReadInt16();		// ?	
			face.styles = Stream.ReadBytes(4);			// switchable lighting info[4]
			face.lightmap = Stream.ReadInt32();			// offset into lightmap lump
			face.area = Stream.ReadSingle();				// face area in units^2
			face.LightmapTextureMinsInLuxels = new int[] { Stream.ReadInt32(), Stream.ReadInt32() };   // texture lighting info
			face.LightmapTextureSizeInLuxels = new int[] { Stream.ReadInt32(), Stream.ReadInt32() };   // texture lighting info
			face.origFace = Stream.ReadInt32();			// original face this was split from
			face.numPrims = Stream.ReadUInt16();		// primitives
			face.firstPrimID = Stream.ReadUInt16();
			face.smoothingGroups = Stream.ReadUInt32();	// lightmap smoothing group
		}
	}
}