using System;

namespace Toe.Editors.Interfaces
{
	public static class CommandExt
	{
		public static ICommand SetValue<T,ARG>(this ICommandHistory history, T target, ARG oldValue, ARG newValue, Action<T, ARG> setValue)
		{
			if (Equals(oldValue, newValue)) return null;
			var setValueCommand = new SetValueCommand<T, ARG>(target, oldValue, newValue, setValue);
			history.RegisterAction(setValueCommand);
			setValueCommand.Redo();
			return setValueCommand;
		}
	}
}