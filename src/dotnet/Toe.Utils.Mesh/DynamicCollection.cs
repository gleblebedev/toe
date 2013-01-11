namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Generic set of parameter, specific to the source of the object.
	/// </summary>
	public class DynamicCollection: IParameterCollection
	{
		private dynamic value;

		public dynamic Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}
	}
}