namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Generic material.
	/// </summary>
	public class SceneMaterial: IMaterial
	{
		/// <summary>
		/// Collection of source specific parameters.
		/// </summary>
		private IParameterCollection parameters;

		#region Implementation of ISceneItem

		/// <summary>
		/// Collection of source specific parameters.
		/// </summary>
		public IParameterCollection Parameters
		{
			get
			{
				return this.parameters ?? (this.parameters = new DynamicCollection());
			}
			set
			{
				this.parameters = value;
			}
		}

		#endregion
	}
}