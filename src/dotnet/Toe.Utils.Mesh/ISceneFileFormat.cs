namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Meshs file file format.
	/// </summary>
	public interface ISceneFileFormat
	{
		ISceneReader CreateReader();

		bool CanLoad(string filename);
	}
}