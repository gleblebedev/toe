using System.Collections.ObjectModel;

namespace Toe.Editor
{
	public class MainEditorWindowOptions
	{
		#region Constants and Fields

		private ObservableCollection<string> recentFiles = new ObservableCollection<string>();

		#endregion

		#region Public Properties

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

		#endregion
	}
}