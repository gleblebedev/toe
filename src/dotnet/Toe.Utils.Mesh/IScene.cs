using System.Collections.Generic;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Generic scene conainer interface.
	/// </summary>
	public interface IScene : ISceneItem, INodeContainer
	{
		IList<IImage> Images { get; }
		IList<IEffect> Effects { get; }
		IList<IMaterial> Materials { get; }
		IList<IMesh> Geometries { get; }
	}
}