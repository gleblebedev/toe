using System.Collections.Generic;

namespace Toe.Resources
{
	public interface IResourceFile:IBasePathProvider
	{
		void Close();

		void Open();

		IList<IResourceFileItem> Items { get; }

		string FilePath { get; }
	}
}