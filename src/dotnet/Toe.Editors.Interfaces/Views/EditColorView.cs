using System;
using System.Drawing;
using System.Windows.Forms;

using Toe.Editors.Interfaces.Bindings;

namespace Toe.Editors.Interfaces.Views
{
	public class EditColorView : UserControl, IView
	{
		readonly DataContextContainer dataContext = new DataContextContainer();

		public EditColorView()
		{
			this.dataContext.DataContextChanged += this.UpdateTextBox;
			this.Height = 16;
			this.Cursor = Cursors.Hand;
			this.Click += OpenColorPicker;
		}

		private void OpenColorPicker(object sender, EventArgs e)
		{
			var cpd = new ColorPickerDialog();
			if (cpd.ShowDialog() == DialogResult.OK)
			{
				
			}
		}

		private void UpdateTextBox(object sender, DataContextChangedEventArgs e)
		{
			if (this.dataContext.Value != null)
			{
				this.BackColor = (Color)this.dataContext.Value;
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