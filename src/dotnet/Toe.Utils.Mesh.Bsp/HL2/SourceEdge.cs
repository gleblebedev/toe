namespace Toe.Utils.Mesh.Bsp.HL2
{
	public struct SourceEdge
	{
		#region Constants and Fields

		public ushort vertex0; // index of the start vertex, must be in [0,numvertices[

		public ushort vertex1; // index of the end vertex,  must be in [0,numvertices[

		#endregion
	};
}