using System.Collections.Generic;

using OpenTK;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Mesh as set of submeshes with common set of vertices.
	/// 
	/// Usualy the implemenation of this interface is not efficient. 
	/// Please use it in content pipeline only! It is NOT recommended to use it in production.
	/// </summary>
	public interface IMesh : ISceneItem, IVertexStreamSource
	{
		#region Public Properties

		Vector3 BoundingBoxMax { get; }

		Vector3 BoundingBoxMin { get; }

		Vector3 BoundingSphereCenter { get; }

		float BoundingSphereR { get; }

		object RenderData { get; set; }

		IList<ISubMesh> Submeshes { get; }

		#endregion

		#region Public Methods and Operators

		void InvalidateBounds();

		#endregion
	}
}