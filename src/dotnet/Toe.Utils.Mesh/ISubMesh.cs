namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Submesh with single material.
	/// </summary>
	public interface ISubMesh: IVertexSource
	{

		IMaterial Material { get; set; }

		string Name { get; }
	}
}