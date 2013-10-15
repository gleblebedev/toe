namespace Toe.Utils.Mesh.Bsp.Q1
{
	public class BaseQ1HL2BspReader : BaseBspReader
	{
		#region Constants and Fields

		protected BspFileHeader header;

		#endregion

		#region Methods

		public BaseQ1HL2BspReader(IStreamConverterFactory streamConverterFactory)
			: base(streamConverterFactory)
		{
		}

		protected override void CreateMesh()
		{
			throw new System.NotImplementedException();
		}

		protected override void ReadHeader()
		{
			this.header.version = this.Stream.ReadUInt32();

			this.header.entities.offset = this.Stream.ReadUInt32();
			this.header.entities.size = this.Stream.ReadUInt32();

			this.header.planes.offset = this.Stream.ReadUInt32();
			this.header.planes.size = this.Stream.ReadUInt32();

			this.header.miptex.offset = this.Stream.ReadUInt32();
			this.header.miptex.size = this.Stream.ReadUInt32();

			this.header.vertices.offset = this.Stream.ReadUInt32();
			this.header.vertices.size = this.Stream.ReadUInt32();

			this.header.visilist.offset = this.Stream.ReadUInt32();
			this.header.visilist.size = this.Stream.ReadUInt32();

			this.header.nodes.offset = this.Stream.ReadUInt32();
			this.header.nodes.size = this.Stream.ReadUInt32();

			this.header.texinfo.offset = this.Stream.ReadUInt32();
			this.header.texinfo.size = this.Stream.ReadUInt32();

			this.header.faces.offset = this.Stream.ReadUInt32();
			this.header.faces.size = this.Stream.ReadUInt32();

			this.header.lightmaps.offset = this.Stream.ReadUInt32();
			this.header.lightmaps.size = this.Stream.ReadUInt32();

			this.header.clipnodes.offset = this.Stream.ReadUInt32();
			this.header.clipnodes.size = this.Stream.ReadUInt32();

			this.header.leaves.offset = this.Stream.ReadUInt32();
			this.header.leaves.size = this.Stream.ReadUInt32();

			this.header.lface.offset = this.Stream.ReadUInt32();
			this.header.lface.size = this.Stream.ReadUInt32();

			this.header.edges.offset = this.Stream.ReadUInt32();
			this.header.edges.size = this.Stream.ReadUInt32();

			this.header.ledges.offset = this.Stream.ReadUInt32();
			this.header.ledges.size = this.Stream.ReadUInt32();

			this.header.models.offset = this.Stream.ReadUInt32();
			this.header.models.size = this.Stream.ReadUInt32();
		}

		protected override void ReadVertices()
		{
		}

		#endregion
	}
}