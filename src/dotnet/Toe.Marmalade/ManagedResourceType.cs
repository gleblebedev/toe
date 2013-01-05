using Toe.Marmalade.IwGx;
using Toe.Resources;

namespace Toe.Marmalade
{
	public class ManagedResourceType<T>:IResourceType where T:Managed
	{
		private readonly IResourceManager resourceManager;

		public ManagedResourceType(IResourceManager resourceManager)
		{
			this.resourceManager = resourceManager;
		}

		public ResourceReference BuildReference(IResourceItem item, bool fileReferencesAllowed)
		{
			var r = new ResourceReference(item.Type, resourceManager, item.Source);
			var val = item.Value as T;
			if (val == null)
			{
				return r;
			}
			if (fileReferencesAllowed)
			{
				if (item.Source.Items.Count == 1)
				{
					r.FileReference = item.Source.FilePath;
					return r;
				}
			}
			if (!string.IsNullOrEmpty(val.Name))
			{
				r.NameReference = val.Name;
				return r;
			}
			r.HashReference = val.NameHash;
			return r;
		}
	}
}