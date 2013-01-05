using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

using Toe.Editors.Interfaces.Bindings;

namespace Toe.Editors.Interfaces.Views
{
	public class EditIntegerView: SingleControlView<TextBox>, IView
	{
		readonly DataContextContainer dataContext = new DataContextContainer();

		public EditIntegerView()
		{
			this.dataContext.DataContextChanged += this.UpdateTextBox;
			this.ViewControl.TextChanged += this.UpdateDataContext;
		}

		private void UpdateDataContext(object sender, EventArgs e)
		{
			int v;
			if (!int.TryParse(this.ViewControl.Text, out v))
			{
				BackColor = Color.Red;
			}
			BackColor = Color.Transparent;
			if (!Equals(this.DataContext.Value, v))
			{
				this.DataContext.Value = v;
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