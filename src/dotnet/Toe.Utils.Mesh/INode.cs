using Toe.Utils.ToeMath;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Generic scene node interface.
	/// </summary>
	public interface INode : ISceneItem, INodeContainer
	{
		#region Public Properties

		IMesh Mesh { get; set; }

		Float4x4 ModelMatrix { get; }

		INodeSkin NodeSkin { get; }

		Float3 Position { get; set; }

		Float4 Rotation { get; set; }

		Float3 Scale { get; set; }

		#endregion
	}
}