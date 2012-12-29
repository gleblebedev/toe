using System;

namespace Toe.Resources
{
	public interface IResourceErrorHandler
	{
		void CanNotRead(string filePath, Exception exception);
	}
}