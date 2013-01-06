using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

using Toe.Editors.Interfaces.Views;

namespace Toe.Editors.Interfaces.Panels
{
	public class StackPanel : Panel //FlowLayoutPanel
	{
		#region Constructors and Destructors

		public StackPanel()
		{
			this.AutoScroll = true;

			//this.FlowDirection = FlowDirection.TopDown;
			//this.WrapContents = false;
		}

		#endregion

		#region Public Properties

		public override LayoutEngine LayoutEngine
		{
			get
			{
				return VerticalStackPanelLayout.Instance;
			}
		}

		#endregion

		#region Public Methods and Operators

		public override Size GetPreferredSize(Size proposedSize)
		{
			int h = 0;
			foreach (Control c in this.Controls)
			{
				// Only apply layout to visible controls.
				if (!c.Visible)
				{
					continue;
				}

				var preferredSize = c.GetPreferredSize(proposedSize);
				h += preferredSize.Height + c.Height + c.Margin.Bottom;
			}
			return new Size(proposedSize.Width, h);
		}

		#endregion
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