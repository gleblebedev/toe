namespace Toe.Utils.Mesh.Bsp.HL2
{
	public class BspReader20 : BspReader19, IBspReader
	{
		#region Methods

		public BspReader20(IStreamConverterFactory streamConverterFactory)
			: base(streamConverterFactory)
		{
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
			sourceLeaf.padding = this.Stream.ReadInt16(); // padding to 4-byte boundary
		}

		protected override void ReadLeaves()
		{
			this.ReadLeaves(32);
		}

		#endregion
	}
}