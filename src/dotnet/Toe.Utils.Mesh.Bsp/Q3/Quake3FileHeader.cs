namespace Toe.Utils.Mesh.Bsp.Q3
{
	public struct Quake3FileHeader
	{
		#region Constants and Fields

		public Quake3FileEntry brushes; //Convex polyhedra used to describe solid space.

		public Quake3FileEntry brushsides; //Brush surfaces.

		public Quake3FileEntry effects; //List of special map effects.

		public Quake3FileEntry entities; //Game-related object descriptions.

		public Quake3FileEntry faces; //Surface geometry.

		public Quake3FileEntry leafbrushes; //Lists of brush indices, one list per leaf.

		public Quake3FileEntry leaffaces; //Lists of face indices, one list per leaf.

		public Quake3FileEntry leafs; //BSP tree leaves.

		public Quake3FileEntry lightmaps; //Packed lightmap data.

		public Quake3FileEntry lightvols; //Local illumination data.

		public uint magic; // magic number ("IBSP")

		public Quake3FileEntry meshverts; //Lists of offsets, one list per mesh.

		public Quake3FileEntry models; //Descriptions of rigid world geometry in map.

		public Quake3FileEntry nodes; //BSP tree nodes.

		public Quake3FileEntry planes; //Planes used by map geometry.

		public Quake3FileEntry textures; //Surface descriptions.

		public uint version;

		public Quake3FileEntry vertexes; //Vertices used to describe faces.

		public Quake3FileEntry visdata; //Cluster-cluster visibility data.

		#endregion
	}
}