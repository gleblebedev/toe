namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Generic material interface.
	/// </summary>
	public interface IImage : ISceneItem
	{
		string GetFilePath();
		byte[] GetRawData();
	}
}