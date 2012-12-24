using System;
using System.Windows.Forms;

using Toe.Editors.Interfaces.Bindings;

namespace Toe.Editors.Interfaces.Views
{
	public class EditBoolView : SingleControlView<CheckBox>, IView
	{
		readonly DataContextContainer dataContext = new DataContextContainer();

		public EditBoolView()
		{
			this.dataContext.DataContextChanged += this.UpdateTextBox;
			this.ViewControl.TextChanged += this.UpdateDataContext;
		}

		private void UpdateDataContext(object sender, EventArgs e)
		{
			if (!Equals(this.DataContext.Value, this.ViewControl.Text))
			{
				this.DataContext.Value = this.ViewControl.Checked;
			}
		}


		private void UpdateTextBox(object sender, DataContextChangedEventArgs e)
		{
			this.ViewControl.Checked = (bool)e.NewValue;
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