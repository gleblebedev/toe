using System.Linq;
using System.Windows.Forms;

namespace Toe.Editors.Interfaces.Views
{
	public class FormRowPanel : HorisontalStackPanel
	{
		
	}
	//public class FormRowPanel : Panel
	//{
	//    protected override void OnControlAdded(ControlEventArgs e)
	//    {
	//        base.OnControlAdded(e);
	//        this.UpdateHeight();
	//    }
	//    private void UpdateHeight()
	//    {
	//        if (this.Dock != DockStyle.Fill)
	//        {
	//            int maxH = (this.Controls.Count == 0) ? 0 : (from Control control in this.Controls select control.Height).Max();
	//            if (this.Height != maxH)
	//                this.Height = maxH;
	//        }
	//    }

	//    protected override void ReorderControls()
	//    {
	//        var fp = this.Parent as FormPanel;
	//        if (fp != null)
	//        {
	//            if (this.Controls.Count == 1)
	//            {
	//                var control = this.Controls[0];
	//                var width = this.Width - control.Margin.Left - control.Margin.Right;
	//                if (control.Width != width)
	//                    control.Width = width;
	//                var left = control.Margin.Left;
	//                if (control.Left != left)
	//                    control.Left = left;
	//            }
	//            else if (this.Controls.Count == 2)
	//            {
	//                var label = this.Controls[0];
	//                var control = this.Controls[1];
	//                var l = this.Width / 2;
	//                var v = this.Width - l;

	//                var width = l - control.Margin.Left - control.Margin.Right;
	//                if (label.Width != width)
	//                    label.Width = width;
	//                var left = label.Margin.Left;
	//                if (label.Left != left)
	//                    label.Left = left;

	//                var i = v - control.Margin.Left - control.Margin.Right;
	//                if (control.Width != i)
	//                    control.Width = i;
	//                var left1 = l + control.Margin.Left;
	//                if (control.Left != left1)
	//                    control.Left = left1;
	//            }
	//        }
	//        this.UpdateHeight();
	//    }
	//}
}