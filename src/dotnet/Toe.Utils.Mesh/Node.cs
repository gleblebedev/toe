using System.Collections.Generic;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Generic node.
	/// </summary>
	public class Node : INode
	{
		/// <summary>
		/// Collection of source specific parameters.
		/// </summary>
		private IParameterCollection parameters;

		#region Implementation of ISceneItem

		/// <summary>
		/// Collection of source specific parameters.
		/// </summary>
		public IParameterCollection Parameters
		{
			get
			{
				return this.parameters ?? (this.parameters = new DynamicCollection());
			}
			set
			{
				this.parameters = value;
			}
		}

		public string Name { get; set; }

		public IMesh Mesh { get; set; }

		#endregion

		readonly IList<INode> nodes = new List<INode>();
		#region Implementation of INodeContainer

		public IList<INode> Nodes
		{
			get
			{
				return nodes;
			}
		}

		#endregion
	}
}