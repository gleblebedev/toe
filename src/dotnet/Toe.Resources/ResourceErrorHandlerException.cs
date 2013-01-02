using System;

namespace Toe.Resources
{
	public class ResourceErrorHandlerException:Exception
	{
		public ResourceErrorHandlerException(string message, Exception exception):base(message,exception)
		{
			
		}
	}
}