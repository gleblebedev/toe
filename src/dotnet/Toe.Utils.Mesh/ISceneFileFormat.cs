namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Meshs file file format.
	/// </summary>
	public interface ISceneFileFormat
	{
		#region Public Methods and Operators

		bool CanLoad(string filename);

		ISceneReader CreateReader();

		#endregion
	}
}