namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Generic set of parameter, specific to the source of the object.
	/// </summary>
	public interface ISceneItem
	{
		IParameterCollection Parameters { get; }
	}
}