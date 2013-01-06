namespace Toe.Resources
{
	public interface IResourceType
	{
		#region Public Methods and Operators

		ResourceReference BuildReference(IResourceItem item, bool fileReferencesAllowed);

		#endregion
	}
}