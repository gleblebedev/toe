using System;
using System.Diagnostics;

namespace Toe.Editors.Interfaces
{
	public class SetValueCommand<T,ARG> : ICommand
	{
		private readonly T target;

		private readonly ARG oldValue;

		private readonly ARG newValue;

		private readonly Action<T, ARG> setValue;

		private readonly Action<T, ARG> undo;

		public SetValueCommand(T target, ARG oldValue, ARG newValue, Action<T, ARG> setValue)
		{
			this.target = target;
			this.oldValue = oldValue;
			this.newValue = newValue;
			this.setValue = setValue;
		
		}

		#region Implementation of ICommand

		public void Redo()
		{
			Trace.WriteLine(string.Format("Redo {0}->{1} at {2}", newValue, oldValue, target));
			setValue(target, newValue);
		}

		public void Undo()
		{
			Trace.WriteLine(string.Format("Undo {0}->{1} at {2}",newValue,oldValue,target));
			setValue(target, oldValue);
		}

		#endregion

		public override string ToString()
		{
			if (newValue != null)
				return newValue.ToString();
			return "(null)";
		}
	}
}