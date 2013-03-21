using System;

using Toe.Resources;

namespace Toe.Marmalade
{
	public class ManagedResourceType<T> : IResourceType
		where T : Managed
	{
		#region Constants and Fields

		private readonly IResourceManager resourceManager;

		#endregion

		#region Constructors and Destructors

		public ManagedResourceType(IResourceManager resourceManager)
		{
			this.resourceManager = resourceManager;
		}

		#endregion

		#region Public Methods and Operators

		public ResourceReference BuildReference(IResourceItem item, bool fileReferencesAllowed)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			var r = new ResourceReference(item.Type, this.resourceManager, item.Source);

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

		#endregion
	}
}