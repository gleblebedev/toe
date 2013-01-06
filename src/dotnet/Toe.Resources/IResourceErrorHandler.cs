using System;

namespace Toe.Resources
{
	public interface IResourceErrorHandler
	{
		#region Public Methods and Operators

		void CanNotRead(string filePath, Exception exception);

		#endregion
	}
}