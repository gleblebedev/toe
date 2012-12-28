using System.Collections.Generic;

namespace Toe.Resources
{
	public interface IResourceFileFormat
	{
		bool CanRead(string filePath);

		IList<IResourceFileItem> Read(string filePath);

		bool CanWrite(string filePath);

		void Write(string filePath, IList<IResourceFileItem> items);
	}
}