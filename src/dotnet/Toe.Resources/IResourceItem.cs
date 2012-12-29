namespace Toe.Resources
{
	public interface IResourceItem
	{
		uint Hash { get; }

		uint Type { get; }

		object Value { get; }
	}
}