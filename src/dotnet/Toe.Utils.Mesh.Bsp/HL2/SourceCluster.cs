using System.Collections.Generic;

namespace Toe.Utils.Mesh.Bsp.HL2
{
	public struct SourceCluster
	{
		#region Constants and Fields

		public List<int> leaves;

		public int offset;

		public int phs;

		public List<int> visiblity;

		#endregion
	}
}