using System;
using System.Globalization;
using System.Windows.Forms;

using Toe.Editors.Interfaces.Bindings;

namespace Toe.Editors.Interfaces.Views
{
	public class EditStringView : SingleControlView<TextBox>, IView
	{
		readonly DataContextContainer dataContext = new DataContextContainer();

		public EditStringView()
		{
			this.dataContext.DataContextChanged += this.UpdateTextBox;
			this.ViewControl.TextChanged += this.UpdateDataContext;
		}

		private void UpdateDataContext(object sender, EventArgs e)
		{
			if (!Equals(this.DataContext.Value, this.ViewControl.Text))
			{
				this.DataContext.Value = this.ViewControl.Text;
			}
		}

		
		private void UpdateTextBox(object sender, DataContextChangedEventArgs e)
		{
			this.ViewControl.Text = string.Format(CultureInfo.InvariantCulture, "{0}", e.NewValue);
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