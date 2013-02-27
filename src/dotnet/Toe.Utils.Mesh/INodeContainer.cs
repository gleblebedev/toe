using System.Collections.Generic;

namespace Toe.Utils.Mesh
{
	public interface INodeContainer
	{
		#region Public Properties

		IList<INode> Nodes { get; }

		#endregion
	}
}