using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

using Toe.Editors.Interfaces.Bindings;

namespace Toe.Editors.Interfaces.Views
{
	public class ButtonView : UserControl, IView
	{
		#region Constants and Fields

		private readonly DataContextContainer dataContext = new DataContextContainer();

		private readonly Button label;

		#endregion

		#region Constructors and Destructors

		public ButtonView()
		{
			this.label = new Button();
			this.label.Click += (s, a) => this.OnClick(a);
			this.label.AutoSize = true;
			this.AutoSize = true;
			this.Height = this.label.Height;
			this.label.SizeChanged += this.LabelSizeChanged;
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

		#region Public Methods and Operators

		public override Size GetPreferredSize(Size proposedSize)
		{
			var preferredSize = this.label.GetPreferredSize(proposedSize);
			if (proposedSize.Width < int.MaxValue && proposedSize.Width > preferredSize.Width)
			{
				preferredSize = new Size(proposedSize.Width, preferredSize.Height);
			}
			return preferredSize;
		}

		#endregion

		#region Methods

		protected override void OnResize(EventArgs e)
		{
			this.label.Width = this.Width;
			base.OnResize(e);
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

		#endregion
	}
}