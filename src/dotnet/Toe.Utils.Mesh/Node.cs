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
		#region Constants and Fields

		protected static PropertyEventArgs PositionEventArgs = Expr.PropertyEventArgs<Node>(x => x.Position);

		protected static PropertyEventArgs RotationEventArgs = Expr.PropertyEventArgs<Node>(x => x.Rotation);

		protected static PropertyEventArgs ScaleEventArgs = Expr.PropertyEventArgs<Node>(x => x.Scale);

		private readonly IList<INode> nodes = new ObservableCollection<INode>();

		private Vector3 position = Vector3.Zero;

		private Quaternion rotation = Quaternion.Identity;

		private Vector3 scale = new Vector3(1.0f, 1.0f, 1.0f);

		private readonly INodeSkin nodeSkin = new NodeSkin();

		#endregion

		#region Public Properties

		public IMesh Mesh { get; set; }

		public Matrix4 ModelMatrix
		{
			get
			{
				return Matrix4.Rotate(this.Rotation) * Matrix4.CreateTranslation(this.Position);
			}
		}

		public INodeSkin NodeSkin
		{
			get
			{
				return nodeSkin;
			}
		}

		public IList<INode> Nodes
		{
			get
			{
				return this.nodes;
			}
		}

		public Vector3 Position
		{
			get
			{
				return this.position;
			}
			set
			{
				if (this.position != value)
				{
					this.RaisePropertyChanging(PositionEventArgs.Changing);
					this.position = value;
					this.RaisePropertyChanged(PositionEventArgs.Changed);
				}
			}
		}

		public Quaternion Rotation
		{
			get
			{
				return this.rotation;
			}
			set
			{
				if (this.rotation != value)
				{
					this.RaisePropertyChanging(RotationEventArgs.Changing);
					this.rotation = value;
					this.RaisePropertyChanged(RotationEventArgs.Changed);
				}
			}
		}

		public Vector3 Scale
		{
			get
			{
				return this.scale;
			}
			set
			{
				if (this.scale != value)
				{
					this.RaisePropertyChanging(ScaleEventArgs.Changing);
					this.scale = value;
					this.RaisePropertyChanged(ScaleEventArgs.Changed);
				}
			}
		}

		#endregion
	}
}