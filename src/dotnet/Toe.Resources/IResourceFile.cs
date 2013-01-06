using System.Collections.Generic;

namespace Toe.Resources
{
	public interface IResourceFile : IBasePathProvider
	{
		#region Public Properties

		string FilePath { get; }

		IList<IResourceFileItem> Items { get; }

		#endregion

		#region Public Methods and Operators

		void Close();

		void Open();

		#endregion
	}
}