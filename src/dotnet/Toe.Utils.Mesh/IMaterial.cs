namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Generic material interface.
	/// </summary>
	public interface IMaterial : ISceneItem
	{
		#region Public Properties

		IEffect Effect { get; set; }

		#endregion
	}
}