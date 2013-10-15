using Toe.Utils.ToeMath;

namespace Toe.Utils.Mesh.Bsp.HL2
{
	public struct SourceModel
	{
		#region Constants and Fields

		public int firstface; // index into face array

		public int headnode; // index into node array

		public Float3 maxs; // bounding box

		public Float3 mins; // bounding box

		public int numfaces; // index into face array

		public Float3 origin; // for sounds or lights

		#endregion
	}
}