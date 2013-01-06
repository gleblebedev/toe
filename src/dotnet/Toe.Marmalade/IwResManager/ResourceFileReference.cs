namespace Toe.Utils.Marmalade.IwResManager
{
	public class ResourceFileReference
	{
		#region Public Properties

		public string Path { get; set; }

		#endregion

		#region Public Methods and Operators

		public override string ToString()
		{
			return this.Path;
		}

		#endregion
	}
}