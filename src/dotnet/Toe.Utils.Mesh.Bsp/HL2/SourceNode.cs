namespace Toe.Utils.Mesh.Bsp.HL2
{
	public struct SourceNode
	{
		public int planenum;         // index into plane array
		public int front;      // negative numbers are -(leafs+1), not nodes
		public int back;      // negative numbers are -(leafs+1), not nodes

		public SourceBoundingBox box;
		public ushort face_id;  // index into face array
		public ushort face_num;   // counting both sides
		public short area;             // If all leaves below this node are in the same area, then
		// this is the area index. If not, this is -1.
		public short paddding;		// pad to 32 bytes length
		
	};
}