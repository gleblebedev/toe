using System;
using System.Diagnostics;

namespace Toe.Editors.Interfaces
{
	public class SetValueCommand<T, ARG> : ICommand
	{
		#region Constants and Fields

		private readonly ARG oldValue;

		private readonly Action<T, ARG> setValue;

		private readonly T target;

		#endregion

		#region Constructors and Destructors

		public SetValueCommand(T target, ARG oldValue, ARG newValue, Action<T, ARG> setValue)
		{
			this.target = target;
			this.oldValue = oldValue;
			this.NewValue = newValue;
			this.setValue = setValue;
		}

		#endregion

		#region Public Properties

		public ARG NewValue { get; set; }

		public ARG OldValue
		{
			get
			{
				return this.oldValue;
			}
		}

		public Action<T, ARG> SetValue
		{
			get
			{
				return this.setValue;
			}
		}

		public T Target
		{
			get
			{
				return this.target;
			}
		}

		#endregion

		#region Public Methods and Operators

		public void Redo()
		{
			Trace.WriteLine(string.Format("Redo {0}->{1} at {2}", this.NewValue, this.OldValue, this.Target));
			this.SetValue(this.Target, this.NewValue);
		}

		public override string ToString()
		{
			if (this.NewValue != null)
			{
				return this.NewValue.ToString();
			}
			return "(null)";
		}

		public void Undo()
		{
			Trace.WriteLine(string.Format("Undo {0}->{1} at {2}", this.NewValue, this.OldValue, this.Target));
			this.SetValue(this.Target, this.OldValue);
		}

		#endregion
	}
}