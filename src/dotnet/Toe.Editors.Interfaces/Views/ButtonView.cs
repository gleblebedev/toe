using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

using Toe.Editors.Interfaces.Bindings;

namespace Toe.Editors.Interfaces.Views
{
	public class ButtonView : UserControl, IView
	{
		DataContextContainer dataContext = new DataContextContainer();

		private Button label;

		public ButtonView()
			: base()
		{
			this.label = new Button();
			this.label.AutoSize = true;
			this.AutoSize = true;
			this.Height = this.label.Height;
			this.label.SizeChanged += this.LabelSizeChanged;
			this.Controls.Add(this.label);
			this.dataContext.DataContextChanged += this.UpdateLabel;
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
		}
		private void LabelSizeChanged(object sender, EventArgs e)
		{
			if (this.Height != this.label.Height)
				this.Height = this.label.Height;
		}

		private void UpdateLabel(object sender, DataContextChangedEventArgs e)
		{
			this.label.Text = string.Format(CultureInfo.InvariantCulture, "{0}", e.NewValue);
		}

		#region Implementation of IView

		public string Text
		{
			get
			{
				if (this.dataContext.Value == null) return null;
				return this.dataContext.Value.ToString();
			}
			set
			{
				this.dataContext.Value = value;
			}
		}

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