using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Toe.Editors.Interfaces.Bindings;

namespace Toe.Editors.Interfaces.Views
{
	public class StringView: UserControl, IView
	{
		DataContextContainer dataContext = new DataContextContainer();

		private Label label;

		public StringView()
		{
			label = new Label();
			label.AutoSize = true;
			label.SizeChanged += LabelSizeChanged;
			this.UpdateMaxWidth();
			this.Controls.Add(label);
			dataContext.DataContextChanged += UpdateLabel;
		}

		private void UpdateMaxWidth()
		{
			var maximumSize = new Size(this.Width, short.MaxValue);
			if (this.label.MaximumSize != maximumSize)
			this.label.MaximumSize = maximumSize;
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			this.UpdateMaxWidth();
		}
		private void LabelSizeChanged(object sender, EventArgs e)
		{
			if (this.Height != label.Height)
			this.Height = label.Height;
		}

		private void UpdateLabel(object sender, DataContextChangedEventArgs e)
		{
			label.Text = string.Format(CultureInfo.InvariantCulture, "{0}", e.NewValue);
		}

		#region Implementation of IView

		public string Text
		{
			get
			{
				if (dataContext.Value == null) return null;
				return dataContext.Value.ToString();
			}
			set
			{
				dataContext.Value  = value;
			}
		}

		public DataContextContainer DataContext
		{
			get
			{
				return dataContext;
			}
		}

		#endregion
	}
}
