using System.Collections.Generic;
using System.Collections.ObjectModel;

using OpenTK;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Generic node.
	/// </summary>
	public class Node : SceneItem, INode
	{
	

		#region Implementation of ISceneItem


		public IMesh Mesh { get; set; }

		private Vector3 position = Vector3.Zero;

		private Quaternion rotation = Quaternion.Identity;

		public Matrix4 ModelMatrix
		{
			get
			{
				return Matrix4.Rotate(rotation) * Matrix4.CreateTranslation(position);
			}
		}

		#endregion

		readonly IList<INode> nodes = new ObservableCollection<INode>();
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