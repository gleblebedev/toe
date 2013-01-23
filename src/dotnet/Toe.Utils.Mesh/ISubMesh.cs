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
	}
}