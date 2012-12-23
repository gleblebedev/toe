using System;
using System.Windows.Forms;

namespace Toe.Editors.Interfaces.Views
{
	public class Panel:UserControl
	{
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			this.ReorderChildControls();
		}
		protected override void OnControlAdded(ControlEventArgs e)
		{
			base.OnControlAdded(e);
			this.ReorderChildControls();
			e.Control.SizeChanged += this.OnChildControlSizeChanged;
		}
		private void OnChildControlSizeChanged(object sender, EventArgs e)
		{
			this.ReorderChildControls();
		}

		protected override void OnControlRemoved(ControlEventArgs e)
		{
			base.OnControlAdded(e);
			e.Control.SizeChanged -= this.OnChildControlSizeChanged;
			this.ReorderChildControls();
		}
		private bool isReordering = false;
		protected virtual void ReorderControls(){}
		protected void ReorderChildControls()
		{
			if (this.Controls.Count == 0)
				return;
			try
			{
				if (this.isReordering) return;
				this.isReordering = true;
				this.ReorderControls();
			}
			finally
			{
				this.isReordering = false;
			}
		}
	}
}