namespace Toe.Utils.Mesh.Bsp.Q1
{
	public struct BspFileHeader
	{
		public uint version;
		public BspFileEntry entities;
		public BspFileEntry planes;
		public BspFileEntry miptex;
		public BspFileEntry vertices;
		public BspFileEntry visilist;
		public BspFileEntry nodes;
		public BspFileEntry texinfo;
		public BspFileEntry faces;
		public BspFileEntry lightmaps;
		public BspFileEntry clipnodes;
		public BspFileEntry leaves;
		public BspFileEntry lface;
		public BspFileEntry edges;
		public BspFileEntry ledges;
		public BspFileEntry models;

	}
}