using System.Collections.ObjectModel;

namespace Toe.Editor
{
	public class AddNewItemFormOptions
	{
		#region Constants and Fields

		private ObservableCollection<string> directoryHistory = new ObservableCollection<string>();

		#endregion

		#region Public Properties

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

		#endregion
	}
}