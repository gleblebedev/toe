using System;
using System.Windows.Forms;

namespace Toe.Editors.Interfaces.Views
{
	public class DelayedLayout
	{
		#region Constants and Fields

		private readonly Control control;

		private bool isReady;

		private bool isScheduled;

		#endregion

		#region Constructors and Destructors

		public DelayedLayout(Control control)
		{
			this.control = control;
			control.Paint += this.OnLoaded;
			control.Invalidate();
		}

		#endregion

		#region Public Events

		public event EventHandler UpdateLayout;

		#endregion

		#region Public Methods and Operators

		public void ScheduleLayoutUpdate()
		{
			if (this.isScheduled)
			{
				return;
			}
			this.isScheduled = true;
			if (!this.isReady)
			{
				return;
			}
			this.control.BeginInvoke((Action)this.RaiseLayoutEvent);
		}

		#endregion

		#region Methods

		protected void OnLoaded(object sender, EventArgs e)
		{
			this.control.Paint -= this.OnLoaded;
			this.isReady = true;
			if (this.isScheduled)
			{
				this.control.BeginInvoke((Action)this.RaiseLayoutEvent);
			}
		}

		private void RaiseLayoutEvent()
		{
			this.isScheduled = false;
			if (this.UpdateLayout != null)
			{
				this.UpdateLayout(this.control, new EventArgs());
			}
		}

		#endregion
	}
}