namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Generic set of parameter, specific to the source of the object.
	/// </summary>
	public class DynamicCollection : IParameterCollection
	{
		#region Public Properties

		public dynamic Value { get; set; }

		#endregion
	}
}