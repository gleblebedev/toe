using System.Windows.Forms;

namespace Toe.Editors.Interfaces.Views
{
	public class HorisontalStackPanel : FlowLayoutPanel
	{
		#region Constructors and Destructors

		public HorisontalStackPanel()
		{
			this.AutoScroll = true;
			this.FlowDirection = FlowDirection.LeftToRight;
			this.WrapContents = false;
		}

		#endregion
	}

	//public class HorisontalStackPanel : Panel
	//{
	//    public HorisontalStackPanel()
	//    {
	//        this.Height = 1;
	//    }
	//    protected override void OnControlAdded(ControlEventArgs e)
	//    {
	//        base.OnControlAdded(e);
	//        this.UpdateHeight();
	//    }
	//    private void UpdateHeight()
	//    {
	//        int maxH = (this.Controls.Count == 0) ? 0 : (from Control control in this.Controls select control.Height).Max();
	//        if (this.Dock != DockStyle.Fill)
	//        {
	//            if (this.Height != maxH)
	//                this.Height = maxH;
	//        }
	//    }
	//    protected override void ReorderControls()
	//    {
	//        this.UpdateHeight();
	//        int x = 0;
	//        foreach (Control control in this.Controls)
	//        {
	//            var left = x + control.Margin.Left;
	//            if (control.Left != left)
	//            control.Left = left;
	//            var top = control.Margin.Top;
	//            if (control.Top != top)
	//            control.Top = top;
	//            //control.Height = this.Height - (control.Margin.Left + control.Margin.Right);
	//            x += control.Width + control.Margin.Left + control.Margin.Right;
	//        }
	//    }
	//}
}