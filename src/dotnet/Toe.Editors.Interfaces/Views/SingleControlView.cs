using System;
using System.Windows.Forms;

namespace Toe.Editors.Interfaces.Views
{
	public abstract class SingleControlView<T> : UserControl
		where T : Control, new()
	{
		#region Constants and Fields

		private readonly T viewControl = new T();

		#endregion

		#region Constructors and Destructors

		public SingleControlView()
		{
			this.Controls.Add(this.viewControl);
			if (this.Height != this.viewControl.Height)
			{
				this.Height = this.viewControl.Height;
			}
			this.viewControl.SizeChanged += this.ResizeControlByTextBox;
		}

		#endregion

		#region Properties

		protected T ViewControl
		{
			get
			{
				return this.viewControl;
			}
		}

		#endregion

		#region Methods

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			if (this.viewControl.Width != this.Width)
			{
				this.viewControl.Width = this.Width;
			}
		}

		private void ResizeControlByTextBox(object sender, EventArgs e)
		{
			if (this.Height != this.viewControl.Height)
			{
				this.Height = this.viewControl.Height;
			}
		}

		#endregion
	}
}