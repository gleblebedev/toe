using System;

namespace Toe.Resources
{
	public class ResourceErrorHandlerException : Exception
	{
		#region Constructors and Destructors

		public ResourceErrorHandlerException(string message, Exception exception)
			: base(message, exception)
		{
		}

		#endregion
	}
}