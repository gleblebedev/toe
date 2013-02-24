namespace Toe.Utils.Mesh.Bsp.Q1
{
	public struct BspFileHeader
	{
		#region Constants and Fields

		public BspFileEntry clipnodes;

		public BspFileEntry edges;

		public BspFileEntry entities;

		public BspFileEntry faces;

		public BspFileEntry leaves;

		public BspFileEntry ledges;

		public BspFileEntry lface;

		public BspFileEntry lightmaps;

		public BspFileEntry miptex;

		public BspFileEntry models;

		public BspFileEntry nodes;

		public BspFileEntry planes;

		public BspFileEntry texinfo;

		public uint version;

		public BspFileEntry vertices;

		public BspFileEntry visilist;

		#endregion
	}
}