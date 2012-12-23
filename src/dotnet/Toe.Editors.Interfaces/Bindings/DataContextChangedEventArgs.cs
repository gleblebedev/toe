using System;

namespace Toe.Editors.Interfaces.Bindings
{
	public class DataContextChangedEventArgs:EventArgs
	{
		private readonly object oldValue;

		private readonly object newValue;

		public DataContextChangedEventArgs(object oldValue, object newValue)
		{
			this.oldValue = oldValue;
			this.newValue = newValue;
		}

		public object OldValue
		{
			get
			{
				return this.oldValue;
			}
		}

		public object NewValue
		{
			get
			{
				return this.newValue;
			}
		}
	}
}