using System;

namespace Toe.Resources
{
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
}