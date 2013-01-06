using System;
using System.Windows.Forms;

using Toe.Editors.Interfaces.Bindings;

namespace Toe.Editors.Interfaces.Views
{
	public class EditBoolView : SingleControlView<CheckBox>, IView
	{
		#region Constants and Fields

		private readonly DataContextContainer dataContext = new DataContextContainer();

		#endregion

		#region Constructors and Destructors

		public EditBoolView()
		{
			this.dataContext.DataContextChanged += this.UpdateTextBox;
			this.ViewControl.CheckedChanged += this.UpdateDataContext;
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

		private void UpdateDataContext(object sender, EventArgs e)
		{
			this.DataContext.Value = this.ViewControl.Checked;
		}

		private void UpdateTextBox(object sender, DataContextChangedEventArgs e)
		{
			this.ViewControl.Checked = (bool)e.NewValue;
		}

		#endregion
	}
}