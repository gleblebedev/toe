using System;
using System.Windows.Forms;

namespace Toe.Editors.Interfaces.Views
{
	//public class Panel:UserControl
	//{
	//    public Panel()
	//    {
	//        delayedLayout = new DelayedLayout(this);
	//        delayedLayout.UpdateLayout += (s, a) => ReorderChildControls();
	//    }
	//    private DelayedLayout delayedLayout;
	//    protected override void OnSizeChanged(EventArgs e)
	//    {
	//        base.OnSizeChanged(e);
	//        delayedLayout.ScheduleLayoutUpdate();
	//        //this.ReorderChildControls();
	//    }
	//    protected override void OnControlAdded(ControlEventArgs e)
	//    {
	//        base.OnControlAdded(e);
	//        //delayedLayout.ScheduleLayoutUpdate();
	//        //this.ReorderChildControls();
	//        e.Control.SizeChanged += this.OnChildControlSizeChanged;
	//    }
	//    private void OnChildControlSizeChanged(object sender, EventArgs e)
	//    {
	//        //delayedLayout.ScheduleLayoutUpdate();
	//        //this.ReorderChildControls();
	//    }

	//    protected override void OnControlRemoved(ControlEventArgs e)
	//    {
	//        base.OnControlAdded(e);
	//        e.Control.SizeChanged -= this.OnChildControlSizeChanged;
	//        //delayedLayout.ScheduleLayoutUpdate();
	//        //this.ReorderChildControls();
	//    }
	//    protected virtual void ReorderControls(){}
	//    protected void ReorderChildControls()
	//    {
	//        if (this.Controls.Count == 0)
	//            return;
	//        this.ReorderControls();
	//    }
	//}
}