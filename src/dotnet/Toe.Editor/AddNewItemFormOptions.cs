using System.Collections.ObjectModel;

namespace Toe.Editor
{
	public class AddNewItemFormOptions
	{
		private ObservableCollection<string> directoryHistory = new ObservableCollection<string>();

		public ObservableCollection<string> DirectoryHistory
		{
			get
			{
				return this.directoryHistory;
			}
			set
			{
				this.directoryHistory = value;
			}
		}
	}
}