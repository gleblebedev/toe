using System;
using System.Globalization;
using System.Windows.Forms;

using Toe.Editors.Interfaces.Bindings;

namespace Toe.Editors.Interfaces.Views
{
	public class EditShortView : SingleControlView<TextBox>, IView
	{
		#region Constants and Fields

		private readonly DataContextContainer dataContext = new DataContextContainer();

		#endregion

		#region Constructors and Destructors

		public EditShortView()
		{
			this.dataContext.DataContextChanged += this.UpdateTextBox;
			this.ViewControl.TextChanged += this.UpdateDataContext;
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
			if (!Equals(this.DataContext.Value, this.ViewControl.Text))
			{
				this.DataContext.Value = short.Parse(this.ViewControl.Text, CultureInfo.InvariantCulture);
			}
		}

		private void UpdateTextBox(object sender, DataContextChangedEventArgs e)
		{
			this.ViewControl.Text = string.Format(CultureInfo.InvariantCulture, "{0}", e.NewValue);
		}

		#endregion
	}

	public class EditFloatView : SingleControlView<TextBox>, IView
	{
		#region Constants and Fields

		private readonly DataContextContainer dataContext = new DataContextContainer();

		#endregion

		#region Constructors and Destructors

		public EditFloatView()
		{
			this.dataContext.DataContextChanged += this.UpdateTextBox;
			this.ViewControl.TextChanged += this.UpdateDataContext;
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
			if (!Equals(this.DataContext.Value, this.ViewControl.Text))
			{
				this.DataContext.Value = float.Parse(this.ViewControl.Text, CultureInfo.InvariantCulture);
			}
		}

		private void UpdateTextBox(object sender, DataContextChangedEventArgs e)
		{
			this.ViewControl.Text = string.Format(CultureInfo.InvariantCulture, "{0}", e.NewValue);
		}

		#endregion
	}
}