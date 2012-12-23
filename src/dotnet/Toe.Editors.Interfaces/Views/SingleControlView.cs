using System;
using System.Windows.Forms;

namespace Toe.Editors.Interfaces.Views
{
	public abstract class SingleControlView<T> : UserControl where T:Control, new()
	{
		private T viewControl = new T();
		protected T ViewControl
		{
			get
			{
				return this.viewControl;
			}
		}
		public SingleControlView()
		{
			this.Controls.Add(this.viewControl);
			this.Height = this.viewControl.Height;
			this.viewControl.SizeChanged += this.ResizeControlByTextBox;
		}
		private void ResizeControlByTextBox(object sender, EventArgs e)
		{
			this.Height = this.viewControl.Height;
		}

	

		protected override void OnSizeChanged(EventArgs e)
		{
			this.viewControl.Width = this.Width;
			base.OnSizeChanged(e);
		}
	}
}