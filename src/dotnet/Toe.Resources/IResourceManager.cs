using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Toe.Resources
{
	public interface IResourceErrorHandler
	{
		void CanNotRead(string filePath, Exception exception);
	}
	public class ResourceErrorHandler : IResourceErrorHandler
	{
		#region Implementation of IResourceErrorHandler

		public virtual void CanNotRead(string filePath, Exception exception)
		{
			if (exception != null)
				throw exception;
			throw new ApplicationException(string.Format("Can't read {0}", filePath));
		}

		#endregion
	}
	public interface IResourceManager : IDisposable
	{
		IResourceFile EnsureFile(string filename);

		IResourceItem ConsumeResource(uint getClassHashCode, uint nameHash);
	}
}
