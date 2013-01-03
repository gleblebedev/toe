using System.Drawing;
using System.Windows.Forms;

using Toe.Editors.Interfaces.Views;

namespace Toe.Editors.Interfaces.Panels
{
	public class StackPanel : Panel //FlowLayoutPanel
	{
		public StackPanel()
		{
			this.AutoScroll = true;
			
			//this.FlowDirection = FlowDirection.TopDown;
			//this.WrapContents = false;
		}
		public override System.Drawing.Size GetPreferredSize(System.Drawing.Size proposedSize)
		{
			int h=0;
			foreach (Control c in Controls)
			{
				// Only apply layout to visible controls.
				if (!c.Visible)
				{
					continue;
				}

				var preferredSize = c.GetPreferredSize(proposedSize);
				h += preferredSize.Height + c.Height + c.Margin.Bottom;
			}
			return new Size(proposedSize.Width,h);
		}
		public override System.Windows.Forms.Layout.LayoutEngine LayoutEngine
		{
			get
			{
				return VerticalStackPanelLayout.Instance;
			}
		}
	}
	//public class StackPanel : Panel
	//{
	//    protected override void ReorderControls()
	//    {
	//        int y = 0;
	//        foreach (Control control in Controls)
	//        {
	//            var top = y + control.Margin.Top;
	//            if (top != control.Top)
	//                control.Top = top;
	//            if (control.Left != control.Margin.Left)
	//                control.Left = control.Margin.Left;
	//            var width = this.Width - (control.Margin.Left + control.Margin.Right);
	//            if (width != control.Width)
	//                control.Width = width;
	//            y += control.Height + control.Margin.Top + control.Margin.Bottom;
	//        }
	//        if (this.Dock != DockStyle.Fill)
	//        {
	//            if (this.Height != y)
	//                this.Height = y;
	//        }
	//    }
	//}
}