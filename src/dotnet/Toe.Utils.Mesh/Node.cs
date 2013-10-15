using System.Collections.Generic;
using System.Collections.ObjectModel;

using Toe.Utils.ToeMath;

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

		private readonly INodeSkin nodeSkin = new NodeSkin();

		private readonly IList<INode> nodes = new ObservableCollection<INode>();

		private Float3 position = Float3.Zero;

		private Float4 rotation = Float4.QuaternionIdentity;

		private Float3 scale = new Float3(1.0f, 1.0f, 1.0f);

		#endregion

		#region Public Properties

		public IMesh Mesh { get; set; }

		public Float4x4 ModelMatrix
		{
			get
			{
				return Float4x4.Rotate(this.Rotation) * Float4x4.CreateTranslation(this.Position);
			}
		}

		public INodeSkin NodeSkin
		{
			get
			{
				return this.nodeSkin;
			}
		}

		public IList<INode> Nodes
		{
			get
			{
				return this.nodes;
			}
		}

		public Float3 Position
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

		public Float4 Rotation
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

		public Float3 Scale
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