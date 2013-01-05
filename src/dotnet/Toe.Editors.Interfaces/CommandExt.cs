using System;

namespace Toe.Editors.Interfaces
{
	public static class CommandExt
	{
		public static ICommand SetValue<T,ARG>(this ICommandHistory history, T target, ARG oldValue, ARG newValue, Action<T, ARG> setValue)
		{
			if (Equals(oldValue, newValue)) return null;

			history.DropRedo();
			var lastCommand = history.Top;
			if (lastCommand != null)
			{
				if (lastCommand is SetValueCommand<T, ARG>)
				{
					var sv = (SetValueCommand<T, ARG>)lastCommand;
					if (Equals(target,sv.Target))
					{
						if (Equals(sv.SetValue,setValue))
						{
							//if (Equals(sv.NewValue, oldValue))
							{
								sv.NewValue = newValue;
								sv.Redo();
								return sv;
							}
						}
					}
				}
			}

			var setValueCommand = new SetValueCommand<T, ARG>(target, oldValue, newValue, setValue);
			history.RegisterAction(setValueCommand);
			setValueCommand.Redo();
			return setValueCommand;
		}
	}
}