using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

using Toe.Editors.Interfaces.Bindings;

namespace Toe.Editors.Interfaces.Views
{
	public class StringView : UserControl, IView
	{
		#region Constants and Fields

		private readonly DataContextContainer dataContext = new DataContextContainer();

		private readonly Label label;

		#endregion

		#region Constructors and Destructors

		public StringView()
		{
			this.label = new Label();
			this.label.AutoSize = true;
			this.label.SizeChanged += this.LabelSizeChanged;
			this.UpdateMaxWidth();
			this.Controls.Add(this.label);
			this.dataContext.DataContextChanged += this.UpdateLabel;
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

		public new string Text
		{
			get
			{
				if (this.dataContext.Value == null)
				{
					return null;
				}
				return this.dataContext.Value.ToString();
			}
			set
			{
				this.dataContext.Value = value;
			}
		}

		#endregion

		#region Methods

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			this.UpdateMaxWidth();
		}

		private void LabelSizeChanged(object sender, EventArgs e)
		{
			if (this.Height != this.label.Height)
			{
				this.Height = this.label.Height;
			}
		}

		private void UpdateLabel(object sender, DataContextChangedEventArgs e)
		{
			this.label.Text = string.Format(CultureInfo.InvariantCulture, "{0}", e.NewValue);
		}

		private void UpdateMaxWidth()
		{
			var maximumSize = new Size(this.Width, short.MaxValue);
			if (this.label.MaximumSize != maximumSize)
			{
				this.label.MaximumSize = maximumSize;
			}
		}

		#endregion
	}
}