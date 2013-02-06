using OpenTK;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Generic scene node interface.
	/// </summary>
	public interface INode : ISceneItem, INodeContainer
	{
		IMesh Mesh { get; set; }

		Matrix4 ModelMatrix { get; }
	}
}