using OpenTK;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Submesh with single material.
	/// </summary>
	public interface ISubMesh : IVertexIndexSource
	{
		#region Public Properties

		Vector3 BoundingBoxMax { get; }

		Vector3 BoundingBoxMin { get; }

		Vector3 BoundingSphereCenter { get; }

		float BoundingSphereR { get; }

		IMaterial Material { get; set; }

		string Name { get; }

		object RenderData { get; set; }

		#endregion

		#region Public Methods and Operators

		void InvalidateBounds();

		#endregion
	}
}