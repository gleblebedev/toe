using OpenTK;

namespace Toe.Utils.Mesh.Bsp.HL2
{
	public struct SourceTexInfo
	{
		#region Constants and Fields

		public float distS; // horizontal offset in texture space

		public float distT; // vertical offset in texture space

		public int flags; // miptex flags + overrides

		public float lm_distS; // horizontal offset in texture space

		public float lm_distT; // vertical offset in texture space

		public Vector3 lm_vectorS; // S vector, horizontal in texture space)

		public Vector3 lm_vectorT; // T vector, vertical in texture space

		public int texdata; // Pointer to texture name, size, etc.

		public Vector3 vectorS; // S vector, horizontal in texture space)

		public Vector3 vectorT; // T vector, vertical in texture space

		#endregion
	}
}