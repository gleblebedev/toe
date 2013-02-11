using OpenTK;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Submesh with single material.
	/// </summary>
	public interface ISubMesh: IVertexIndexSource
	{

		IMaterial Material { get; set; }

		string Name { get; }

		object RenderData { get; set; }

		void InvalidateBounds();

		Vector3 BoundingBoxMin { get; }
		Vector3 BoundingBoxMax { get; }
		Vector3 BoundingSphereCenter { get; }
		float BoundingSphereR { get; }
	}
}