using System.Collections.Generic;

using Toe.Utils.ToeMath;

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

		Float3 BoundingBoxMax { get; }

		Float3 BoundingBoxMin { get; }

		Float3 BoundingSphereCenter { get; }

		float BoundingSphereR { get; }

		object RenderData { get; set; }

		IList<ISubMesh> Submeshes { get; }

		#endregion

		#region Public Methods and Operators

		void InvalidateBounds();

		#endregion
	}
}