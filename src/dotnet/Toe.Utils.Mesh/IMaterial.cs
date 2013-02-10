namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Generic material interface.
	/// </summary>
	public interface IMaterial:ISceneItem
	{
		IEffect Effect { get; set; }
	}
}