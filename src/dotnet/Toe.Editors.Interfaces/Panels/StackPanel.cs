using System.Linq;
using System.Windows.Forms;

namespace Toe.Editors.Interfaces.Views
{
	public class FormPanel : StackPanel
	{
	}
	public class FormRowPanel : Panel
	{
		protected override void OnControlAdded(ControlEventArgs e)
		{
			base.OnControlAdded(e);
			UpdateHeight();
		}
		private void UpdateHeight()
		{
			int maxH = (this.Controls.Count == 0) ? 0:(from Control control in this.Controls select control.Height).Max();
			this.Height = maxH;
		}

		protected override void ReorderControls()
		{
			var fp = this.Parent as FormPanel;
			if (fp != null)
			{
				if (this.Controls.Count == 1)
				{
					var control = this.Controls[0];
					control.Width = this.Width-control.Margin.Left-control.Margin.Right;
					control.Left = control.Margin.Left;
				}
				else if (this.Controls.Count == 2)
				{
					var label = this.Controls[0];
					var control = this.Controls[1];
					var l = this.Width / 2;
					var v = this.Width - l;
					label.Width = l - control.Margin.Left - control.Margin.Right;
					label.Left = label.Margin.Left;
					control.Width = v - control.Margin.Left - control.Margin.Right;
					control.Left = l + control.Margin.Left;
				}
			}
			UpdateHeight();
		}
	}
	public class HorisontalStackPanel : Panel
	{
		protected override void ReorderControls()
		{
			int x = 0;
			foreach (Control control in Controls)
			{
				control.Left = x + control.Margin.Left;
				control.Top = control.Margin.Top;
				//control.Height = this.Height - (control.Margin.Left + control.Margin.Right);
				x += control.Width + control.Margin.Left + control.Margin.Right;
			}
		}
	}
	public class StackPanel : Panel
	{
		protected override void ReorderControls()
		{
			int y = 0;
			foreach (Control control in Controls)
			{
				control.Top = y + control.Margin.Top;
				control.Left = control.Margin.Left;
				control.Width = this.Width - (control.Margin.Left + control.Margin.Right);
				y += control.Height + control.Margin.Top + control.Margin.Bottom;
			}
		}
	}
}