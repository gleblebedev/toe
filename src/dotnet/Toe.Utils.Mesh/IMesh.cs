using System.Collections.Generic;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Mesh as set of submeshes with common set of vertices.
	/// 
	/// Usualy the implemenation of this interface is not efficient. 
	/// Please use it in content pipeline only! It is NOT recommended to use it in production.
	/// </summary>
	public interface IMesh : ISceneItem, IVertexSource
	{
		IList<ISubMesh> Submeshes { get; }

		ISubMesh CreateSubmesh();

		string Name { get; }

		uint NameHash { get; }


	}
}