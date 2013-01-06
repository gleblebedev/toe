using System.Collections.Generic;

namespace Toe.Resources
{
	public interface IResourceFileFormat
	{
		#region Public Methods and Operators

		bool CanRead(string filePath);

		bool CanWrite(string filePath);

		IList<IResourceFileItem> Read(string filePath, IResourceFile resourceFile);

		void Write(string filePath, IList<IResourceFileItem> items);

		#endregion
	}
}