using System.Collections.ObjectModel;

namespace Toe.Editor
{
	public class MainEditorWindowOptions
	{
		private ObservableCollection<string> recentFiles = new ObservableCollection<string>();

		public ObservableCollection<string> RecentFiles
		{
			get
			{
				return this.recentFiles;
			}
			set
			{
				this.recentFiles = value;
			}
		}
	}
}