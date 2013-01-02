namespace Toe.Utils.Marmalade.IwResManager
{
	public class ResourceFileReference
	{
		#region Public Properties

		public string Path { get; set; }

		#endregion

		public override string ToString()
		{
			return Path;
		}
	}
}