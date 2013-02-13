using OpenTK;

namespace Toe.Utils.Mesh.Bsp.HL2
{
	public struct SourceModel
	{
		public Vector3 mins, maxs;		// bounding box
		public Vector3 origin;			// for sounds or lights
		public int headnode;		// index into node array
		public int firstface, numfaces;	// index into face array
	}
}