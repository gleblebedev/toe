using System;
using System.Collections.Generic;

namespace Toe.Resources
{
	public interface IResourceManager : IDisposable
	{
		#region Public Methods and Operators

		IResourceItem ConsumeResource(uint type, uint nameHash);

		IResourceFile EnsureFile(string filename);

		object FindResource(uint type, uint hashReference);

		IList<IResourceItem> GetAllResourcesOfType(uint type);

		void ProvideResource(uint type, uint nameHash, object item, IResourceFile source);

		void ReleaseResource(uint type, uint nameHash);

		void RetractResource(uint type, uint nameHash, object item, IResourceFile source);

		#endregion
	}
}