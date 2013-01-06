using System;
using System.Drawing;
using System.Windows.Forms;

using Toe.Editors.Interfaces.Bindings;
using Toe.Editors.Interfaces.Dialogs;

namespace Toe.Editors.Interfaces.Views
{
	public class EditColorView : UserControl, IView
	{
		#region Constants and Fields

		private readonly DataContextContainer dataContext = new DataContextContainer();

		#endregion

		#region Constructors and Destructors

		public EditColorView()
		{
			this.dataContext.DataContextChanged += this.UpdateTextBox;
			this.Height = 16;
			this.Cursor = Cursors.Hand;
			this.Click += this.OpenColorPicker;
		}

		#endregion

		#region Public Properties

		public DataContextContainer DataContext
		{
			get
			{
				return this.dataContext;
			}
		}

		#endregion

		#region Methods

		private void OpenColorPicker(object sender, EventArgs e)
		{
			var value = (Color)this.dataContext.Value;
			var cpd = new ColorPickerDialog(value);
			if (cpd.ShowDialog() == DialogResult.OK)
			{
				this.dataContext.Value = cpd.SelectedColor;
			}
		}

		private void UpdateTextBox(object sender, DataContextChangedEventArgs e)
		{
			if (this.dataContext.Value != null)
			{
				this.BackColor = (Color)this.dataContext.Value;
			}
		}

		#endregion
	}
}