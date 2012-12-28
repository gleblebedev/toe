using System.Collections.Generic;

namespace Toe.Resources
{
	public interface IResourceFile
	{
		void Close();

		void Open();

		IList<IResourceFileItem> Items { get; }
	}
}