namespace Toe.Resources
{
	public interface IResourceType
	{
		ResourceReference BuildReference(IResourceItem item, bool fileReferencesAllowed);
	}
}