using OpenTK;

namespace Toe.Utils.Mesh.Bsp.HL2
{
	//http://fal.xrea.jp/plugin/SourceSDK/bspfile_8h-source.html

	//    00058 #define MIN_MAP_DISP_POWER              2       // Minimum and maximum power a displacement can be.
	//00059 #define MAX_MAP_DISP_POWER              4       
	//00060 
	//00061 // Max # of neighboring displacement touching a displacement's corner.
	//00062 #define MAX_DISP_CORNER_NEIGHBORS       4
	//00063 
	//00064 #define NUM_DISP_POWER_VERTS(power)     ( ((1 << (power)) + 1) * ((1 << (power)) + 1) )
	//00065 #define NUM_DISP_POWER_TRIS(power)      ( (1 << (power)) * (1 << (power)) * 2 )
	//00066 
	//00067 #define MAX_MAP_DISPINFO                2048
	//00068 #define MAX_MAP_DISP_VERTS              ( MAX_MAP_DISPINFO * ((1<<MAX_MAP_DISP_POWER)+1) * ((1<<MAX_MAP_DISP_POWER)+1) )
	//00069 #define MAX_MAP_DISP_TRIS               ( (1 << MAX_MAP_DISP_POWER) * (1 << MAX_MAP_DISP_POWER) * 2 )
	//00070 #define MAX_DISPVERTS                   NUM_DISP_POWER_VERTS( MAX_MAP_DISP_POWER )
	//00071 #define MAX_DISPTRIS                    NUM_DISP_POWER_TRIS( MAX_MAP_DISP_POWER )
	//enum { ALLOWEDVERTS_SIZE = PAD_NUMBER( MAX_DISPVERTS, 32 ) / 32 };
	public struct SourceDisplacementInfo
	{
		#region Constants and Fields

		public int DispTriStart; // Index into LUMP_DISP_TRIS.

		public int DispVertStart; // Index into LUMP_DISP_VERTS.

		public int LightmapAlphaStart; // Index into ddisplightmapalpha.

		public int LightmapSamplePositionStart; // Index into LUMP_DISP_LIGHTMAP_SAMPLE_POSITIONS.

		public ushort MapFace; // Which map face this displacement comes from.

		public int contents; // surface contents

		public int minTess; // minimum tesselation allowed

		public int power; // power - indicates size of surface (2^power	1)

		public float smoothingAngle; // lighting smoothing angle

		public Vector3 startPosition; // start position used for orientation

		#endregion

		//CDispNeighbor		EdgeNeighbors[4];	// Indexed by NEIGHBOREDGE_ defines.
		//CDispCornerNeighbors	CornerNeighbors[4];	// Indexed by CORNER_ defines.
		//ulong		AllowedVerts[ALLOWEDVERTS_SIZE];	// active verticies
	}

	public struct CDispSubNeighbor
	{
		// 0xFFFF if there is no neighbor here.

		#region Constants and Fields

		private byte m_NeighborOrientation; // (CCW) rotation of the neighbor wrt this displacement.

		// These use the NeighborSpan type.

		private byte m_NeighborSpan; // Where we fit onto our neighbor.

		private byte m_Span; // Where the neighbor fits onto this side of our displacement.

		private ushort m_iNeighbor; // This indexes into ddispinfos.

		#endregion
	}

	public struct CDispNeighbor
	{
		//CDispSubNeighbor        m_SubNeighbors[2];
	}

	public struct CDispCornerNeighbors
	{
		//ushort  m_Neighbors[MAX_DISP_CORNER_NEIGHBORS]; // indices of neighbors.
		//byte m_nNeighbors;
	}

	public struct SourceDisplacementVertex
	{
		#region Constants and Fields

		public float alpha; // "per vertex" alpha values.

		public float dist; // Displacement distances.

		public Vector3 vec; // Vector field defining displacement volume.

		#endregion
	}

	public struct SourceDisplacementTriangle
	{
		#region Constants and Fields

		public ushort Tags;

		#endregion
	}

	public struct SourceFace
	{
		#region Constants and Fields

		public int[] LightmapTextureMinsInLuxels; // texture lighting info

		public int[] LightmapTextureSizeInLuxels; // texture lighting info

		public float area; // face area in units^2

		public short dispinfo; // displacement info

		public ushort firstPrimID;

		public int firstedge; // index into surfedges	

		public int lightmap; // offset into lightmap lump

		/// <summary>
		/// Model ID
		/// Used to filter faces in leaves since model faces are belong to models
		/// </summary>
		public int modelId;

		public ushort numPrims; // primitives

		public short numedges; // number of surfedges

		public byte onNode; // 1 of on node, 0 if in leaf

		public int origFace; // original face this was split from

		public ushort planenum; // the plane number

		public byte side; // faces opposite to the node's plane direction

		public uint smoothingGroups; // lightmap smoothing group

		public byte[] styles; // switchable lighting info[4]

		public short surfaceFogVolumeID; // ?	

		public short texinfo; // texture info

		#endregion

		// this define the start of the face light map
	}
}