namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Generic set of parameter, specific to the source of the object.
	/// </summary>
	public interface ISceneItem
	{
		#region Public Properties

		/// <summary>
		/// Identifier of the scene item.
		/// </summary>
		string Id { get; set; }

		/// <summary>
		/// Name of the scene item.
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Set of additional parameters beyond default set.
		/// </summary>
		IParameterCollection Parameters { get; }

		/// <summary>
		/// Render specific data.
		/// </summary>
		object RenderData { get; set; }

		#endregion
	}
}