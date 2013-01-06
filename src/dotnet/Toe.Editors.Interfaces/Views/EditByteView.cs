using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

using Toe.Editors.Interfaces.Bindings;

namespace Toe.Editors.Interfaces.Views
{
	public class EditByteView : SingleControlView<TextBox>, IView
	{
		#region Constants and Fields

		private readonly DataContextContainer dataContext = new DataContextContainer();

		#endregion

		#region Constructors and Destructors

		public EditByteView()
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
			byte v;
			if (!byte.TryParse(this.ViewControl.Text, out v))
			{
				this.BackColor = Color.Red;
			}
			this.BackColor = Color.Transparent;
			if (!Equals(this.DataContext.Value, v))
			{
				this.DataContext.Value = v;
			}
		}

		private void UpdateTextBox(object sender, DataContextChangedEventArgs e)
		{
			this.ViewControl.Text = string.Format(CultureInfo.InvariantCulture, "{0}", e.NewValue);
		}

		#endregion
	}

	public class EditIntView : SingleControlView<TextBox>, IView
	{
		#region Constants and Fields

		private readonly DataContextContainer dataContext = new DataContextContainer();

		#endregion

		#region Constructors and Destructors

		public EditIntView()
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
				this.DataContext.Value = int.Parse(this.ViewControl.Text, CultureInfo.InvariantCulture);
			}
		}

		private void UpdateTextBox(object sender, DataContextChangedEventArgs e)
		{
			this.ViewControl.Text = string.Format(CultureInfo.InvariantCulture, "{0}", e.NewValue);
		}

		#endregion
	}
}