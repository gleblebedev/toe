namespace Toe.Utils.Mesh.Bsp.HL2
{
	public struct SourceFace
	{
		public ushort planenum;		// the plane number
		public byte side;			// faces opposite to the node's plane direction
		public byte onNode; 			// 1 of on node, 0 if in leaf
		public int firstedge;			// index into surfedges	
		public short numedges;			// number of surfedges
		public short texinfo;			// texture info
		public short dispinfo;			// displacement info
		public short surfaceFogVolumeID;		// ?	
		public byte[] styles;			// switchable lighting info[4]
		public int lightmap;			// offset into lightmap lump
		public float area;				// face area in units^2
		public int[] LightmapTextureMinsInLuxels;   // texture lighting info
		public int[] LightmapTextureSizeInLuxels;   // texture lighting info
		public int origFace;			// original face this was split from
		public ushort numPrims;		// primitives
		public ushort firstPrimID;
		public uint smoothingGroups;	// lightmap smoothing group

		/// <summary>
		/// Model ID
		/// Used to filter faces in leaves since model faces are belong to models
		/// </summary>
		public int modelId;
		// this define the start of the face light map

	}
}