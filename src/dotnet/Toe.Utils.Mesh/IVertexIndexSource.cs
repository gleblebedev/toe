using System.Collections.Generic;

namespace Toe.Utils.Mesh
{
	public interface IVertexIndexSource: IEnumerable<int>
	{
		int Count { get; }
		VertexSourceType VertexSourceType { get; }
	}
}