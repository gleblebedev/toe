using System;
using System.Windows.Forms;

namespace Toe.Editors.Interfaces.Views
{
	public class DelayedLayout
	{
		private readonly Control control;

		private bool isScheduled = false;

		private bool isReady = false;
		public DelayedLayout(Control control)
		{
			this.control = control;
			control.Paint += OnLoaded;
			control.Invalidate();
		}
		protected void OnLoaded(object sender, EventArgs e)
		{
			control.Paint -= OnLoaded;
			isReady = true;
			if (isScheduled)
				this.control.BeginInvoke((Action)this.RaiseLayoutEvent);
		}
		public void ScheduleLayoutUpdate()
		{
			if (this.isScheduled)
				return;
			this.isScheduled = true;
			if (!isReady)
				return;
			this.control.BeginInvoke((Action)this.RaiseLayoutEvent);
		}
		void RaiseLayoutEvent()
		{
			this.isScheduled = false;
			if (this.UpdateLayout != null) this.UpdateLayout(this.control, new EventArgs());
		}

		public event EventHandler UpdateLayout;
	}
}