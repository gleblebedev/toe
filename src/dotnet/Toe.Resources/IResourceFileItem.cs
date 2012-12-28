namespace Toe.Resources
{
	public interface IResourceFileItem
	{
		object Resource { get; }

		string Name { get; }
		uint NameHash { get; }
	}
}