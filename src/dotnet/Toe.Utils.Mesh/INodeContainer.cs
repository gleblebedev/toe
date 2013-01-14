using System.Collections.Generic;

namespace Toe.Utils.Mesh
{
	public interface INodeContainer
	{
		IList<INode> Nodes { get; }
	}
}