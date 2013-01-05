using System.Collections.Generic;
using System.ComponentModel;

namespace Toe.Editors.Interfaces
{
	public class CommandHistory:ICommandHistory
	{
		readonly List<ICommand> commands = new List<ICommand>();

		private int position = 0;

		private int lockCounter;

		public void Redo()
		{
			if (position < commands.Count)
			{
				commands[position].Redo();
				++position;
				if (position == commands.Count)
				{
					this.RaisePropertyChanged("CanRedo");
				}
				if (position == 1)
				{
					this.RaisePropertyChanged("CanUndo");
				}
			}
		}

		public void Undo()
		{
			if (position > 0)
			{
				--position;
				commands[position].Undo();
				if (position == 0)
				{
					this.RaisePropertyChanged("CanUndo");
				}
				if (position == commands.Count-1)
				{
					this.RaisePropertyChanged("CanRedo");
				}
			}
		}

		public bool CanRedo
		{
			get
			{
				return (position < commands.Count);
			}
		}

		public bool CanUndo
		{
			get
			{
				return (position > 0 && commands.Count > 0);
			}
		}

		public ICommand RegisterAction(ICommand action)
		{
			if (IsLocked) return action;
			this.DropRedo();
			
			commands.Add(action);
			++position;
			this.RaisePropertyChanged("CanUndo");
			this.RaisePropertyChanged("CanRedo");
			return action;
		}

		public void Clear()
		{
			position = 0;
			commands.Clear();
			this.RaisePropertyChanged("CanUndo");
			this.RaisePropertyChanged("CanRedo");
		}

		public void Lock()
		{
			++this.lockCounter;
		}

		public void Unlock()
		{
			--lockCounter;
		}

		public bool IsLocked
		{
			get
			{
				return lockCounter > 0;
			}
		}

		public ICommand Top
		{
			get
			{
				if (position > 0 && position <= commands.Count) return commands[position-1];
				return null;
			}
		}

		public void DropRedo()
		{
			if (position < commands.Count)
			{
				while (position < commands.Count)
				{
					commands.RemoveAt(commands.Count - 1);
				}
				this.RaisePropertyChanged("CanRedo");
			}
		}

		private void RaisePropertyChanged(string property)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(property));
			}
		}

		#region Implementation of INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}
}