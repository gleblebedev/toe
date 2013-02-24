namespace Toe.Utils.Mesh.Bsp.HL2
{
	public class BspReader19 : BspReader17, IBspReader
	{
		#region Methods

		protected override void ReadFace(ref SourceFace face)
		{
			face.planenum = this.Stream.ReadUInt16(); // the plane number
			face.side = (byte)this.Stream.ReadByte(); // faces opposite to the node's plane direction
			face.onNode = (byte)this.Stream.ReadByte(); // 1 of on node, 0 if in leaf
			face.firstedge = this.Stream.ReadInt32(); // index into surfedges	
			face.numedges = this.Stream.ReadInt16(); // number of surfedges
			face.texinfo = this.Stream.ReadInt16(); // texture info
			face.dispinfo = this.Stream.ReadInt16(); // displacement info
			face.surfaceFogVolumeID = this.Stream.ReadInt16(); // ?	
			face.styles = this.Stream.ReadBytes(4); // switchable lighting info[4]
			face.lightmap = this.Stream.ReadInt32(); // offset into lightmap lump
			face.area = this.Stream.ReadSingle(); // face area in units^2
			face.LightmapTextureMinsInLuxels = new[] { this.Stream.ReadInt32(), this.Stream.ReadInt32() };
				// texture lighting info
			face.LightmapTextureSizeInLuxels = new[] { this.Stream.ReadInt32(), this.Stream.ReadInt32() };
				// texture lighting info
			face.origFace = this.Stream.ReadInt32(); // original face this was split from
			face.numPrims = this.Stream.ReadUInt16(); // primitives
			face.firstPrimID = this.Stream.ReadUInt16();
			face.smoothingGroups = this.Stream.ReadUInt32(); // lightmap smoothing group
		}

		protected override void ReadFaces()
		{
			this.SeekEntryAt(this.header.Faces.offset);
			int size = this.EvalNumItems(this.header.Faces.size, 56);
			this.faces = new SourceFace[size];
			for (int i = 0; i < size; ++i)
			{
				this.ReadFace(ref this.faces[i]);
			}
			this.AssertStreamPossition(this.header.Faces.size + this.header.Faces.offset);
		}

		protected override void ReadLeaf(ref SourceLeaf sourceLeaf)
		{
			sourceLeaf.contents = this.Stream.ReadInt32(); // OR of all brushes (not needed?)
			sourceLeaf.cluster = this.Stream.ReadInt16(); // cluster this leaf is in
			sourceLeaf.area_flags = this.Stream.ReadInt16(); // area this leaf is in
			this.ReadBBox(ref sourceLeaf.box);
			sourceLeaf.firstleafface = this.Stream.ReadUInt16(); // index into leaffaces
			sourceLeaf.numleaffaces = this.Stream.ReadUInt16();
			sourceLeaf.firstleafbrush = this.Stream.ReadUInt16(); // index into leafbrushes
			sourceLeaf.numleafbrushes = this.Stream.ReadUInt16();
			sourceLeaf.leafWaterDataID = this.Stream.ReadInt16(); // -1 for not in water
			this.ReadCompressedLightCube(ref sourceLeaf.ambientLighting);
			sourceLeaf.padding = this.Stream.ReadInt16(); // padding to 4-byte boundary
		}

		protected override void ReadLeaves()
		{
			this.ReadLeaves(56);
		}

		#endregion
	}
}