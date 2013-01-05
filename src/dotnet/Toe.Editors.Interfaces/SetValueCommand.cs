using System;
using System.Diagnostics;

namespace Toe.Editors.Interfaces
{
	public class SetValueCommand<T,ARG> : ICommand
	{
		private readonly T target;

		private readonly ARG oldValue;

		private ARG newValue;

		private readonly Action<T, ARG> setValue;

		public SetValueCommand(T target, ARG oldValue, ARG newValue, Action<T, ARG> setValue)
		{
			this.target = target;
			this.oldValue = oldValue;
			this.newValue = newValue;
			this.setValue = setValue;
		
		}

		public T Target
		{
			get
			{
				return this.target;
			}
		}

		public ARG OldValue
		{
			get
			{
				return this.oldValue;
			}
		}

		public ARG NewValue
		{
			get
			{
				return this.newValue;
			}
			set
			{
				this.newValue = value;
			}
		}

		public Action<T, ARG> SetValue
		{
			get
			{
				return this.setValue;
			}
		}

		#region Implementation of ICommand

		public void Redo()
		{
			Trace.WriteLine(string.Format("Redo {0}->{1} at {2}", this.NewValue, this.OldValue, this.Target));
			this.SetValue(this.Target, this.NewValue);
		}

		public void Undo()
		{
			Trace.WriteLine(string.Format("Undo {0}->{1} at {2}",this.NewValue,this.OldValue,this.Target));
			this.SetValue(this.Target, this.OldValue);
		}

		#endregion

		public override string ToString()
		{
			if (this.NewValue != null)
				return this.NewValue.ToString();
			return "(null)";
		}
	}
}