using System.Windows.Forms;

using Toe.Editors.Interfaces.Bindings;

namespace Toe.Editors.Interfaces.Views
{
	public class EditEnumView : UserControl, IView
	{
		readonly DataContextContainer dataContext = new DataContextContainer();

		public EditEnumView()
		{
			
			this.dataContext.DataContextChanged += this.UpdateTextBox;
			this.Height = 16;
		}

		private void UpdateTextBox(object sender, DataContextChangedEventArgs e)
		{
			if (this.dataContext.Value != null)
			{
			}
		}


		#region Implementation of IView

		public DataContextContainer DataContext
		{
			get
			{
				return this.dataContext;
			}
		}

		#endregion
	}
}