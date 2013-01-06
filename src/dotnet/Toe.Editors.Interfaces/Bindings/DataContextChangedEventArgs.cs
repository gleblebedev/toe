using System;

namespace Toe.Editors.Interfaces.Bindings
{
	public class DataContextChangedEventArgs : EventArgs
	{
		#region Constants and Fields

		private readonly object newValue;

		private readonly object oldValue;

		#endregion

		#region Constructors and Destructors

		public DataContextChangedEventArgs(object oldValue, object newValue)
		{
			this.oldValue = oldValue;
			this.newValue = newValue;
		}

		#endregion

		#region Public Properties

		public object NewValue
		{
			get
			{
				return this.newValue;
			}
		}

		public object OldValue
		{
			get
			{
				return this.oldValue;
			}
		}

		#endregion
	}
}