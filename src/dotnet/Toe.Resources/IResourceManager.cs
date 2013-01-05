using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Toe.Resources
{
	public interface IResourceManager : IDisposable
	{
		IResourceFile EnsureFile(string filename);

		IResourceItem ConsumeResource(uint type, uint nameHash);
		void ReleaseResource(uint type, uint nameHash);

		void ProvideResource(uint type, uint nameHash, object item, IResourceFile source);
		void RetractResource(uint type, uint nameHash, object item, IResourceFile source);

		object FindResource(uint type, uint hashReference);

		IList<IResourceItem> GetAllResourcesOfType(uint type);
	}
}
