using System;

namespace Toe.Resources
{
	public class ResourceErrorHandler : IResourceErrorHandler
	{
		#region Implementation of IResourceErrorHandler

		public virtual void CanNotRead(string filePath, Exception exception)
		{
			throw new ResourceErrorHandlerException(string.Format("Can't read {0}", filePath), exception);
		}

		#endregion
	}
}