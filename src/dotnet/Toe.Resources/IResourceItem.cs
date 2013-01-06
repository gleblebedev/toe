namespace Toe.Resources
{
	public interface IResourceItem
	{
		#region Public Properties

		uint Hash { get; }

		IResourceFile Source { get; }

		uint Type { get; }

		object Value { get; }

		#endregion
	}
}