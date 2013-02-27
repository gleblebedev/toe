namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Generic material interface.
	/// </summary>
	public interface IImage : ISceneItem
	{
		#region Public Methods and Operators

		string GetFilePath();

		byte[] GetRawData();

		#endregion
	}
}