using OpenTK;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Generic scene node interface.
	/// </summary>
	public interface INode : ISceneItem, INodeContainer
	{
		IMesh Mesh { get; set; }

		Vector3 Position { get; set; }
		Vector3 Scale { get; set; }
		Quaternion Rotation { get; set; }
		Matrix4 ModelMatrix { get; }

		INodeSkin NodeSkin { get; }
	}
}