using System.Collections.Generic;

namespace Toe.Utils.Mesh.Bsp.HL2
{
	public struct  SourceCluster
	{
		public int offset;
		public int phs;
		public List<int> lists;
		public List<int> visiblity;
	}
}