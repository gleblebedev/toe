using OpenTK;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Generic scene node interface.
	/// </summary>
	public interface INode : ISceneItem, INodeContainer
	{
		#region Public Properties

		IMesh Mesh { get; set; }

		Matrix4 ModelMatrix { get; }

		INodeSkin NodeSkin { get; }

		Vector3 Position { get; set; }

		Quaternion Rotation { get; set; }

		Vector3 Scale { get; set; }

		#endregion
	}
}