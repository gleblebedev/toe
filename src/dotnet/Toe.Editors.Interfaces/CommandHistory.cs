using System.Collections.Generic;
using System.ComponentModel;

namespace Toe.Editors.Interfaces
{
	public class CommandHistory : ICommandHistory
	{
		#region Constants and Fields

		private readonly List<ICommand> commands = new List<ICommand>();

		private int lockCounter;

		private int position;

		#endregion

		#region Public Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Public Properties

		public bool CanRedo
		{
			get
			{
				return (this.position < this.commands.Count);
			}
		}

		public bool CanUndo
		{
			get
			{
				return (this.position > 0 && this.commands.Count > 0);
			}
		}

		public bool IsLocked
		{
			get
			{
				return this.lockCounter > 0;
			}
		}

		public ICommand Top
		{
			get
			{
				if (this.position > 0 && this.position <= this.commands.Count)
				{
					return this.commands[this.position - 1];
				}
				return null;
			}
		}

		#endregion

		#region Public Methods and Operators

		public void Clear()
		{
			this.position = 0;
			this.commands.Clear();
			this.RaisePropertyChanged("CanUndo");
			this.RaisePropertyChanged("CanRedo");
		}

		public void DropRedo()
		{
			if (this.position < this.commands.Count)
			{
				while (this.position < this.commands.Count)
				{
					this.commands.RemoveAt(this.commands.Count - 1);
				}
				this.RaisePropertyChanged("CanRedo");
			}
		}

		public void Lock()
		{
			++this.lockCounter;
		}

		public void Redo()
		{
			if (this.position < this.commands.Count)
			{
				this.commands[this.position].Redo();
				++this.position;
				if (this.position == this.commands.Count)
				{
					this.RaisePropertyChanged("CanRedo");
				}
				if (this.position == 1)
				{
					this.RaisePropertyChanged("CanUndo");
				}
			}
		}

		public ICommand RegisterAction(ICommand action)
		{
			if (this.IsLocked)
			{
				return action;
			}
			this.DropRedo();

			this.commands.Add(action);
			++this.position;
			this.RaisePropertyChanged("CanUndo");
			this.RaisePropertyChanged("CanRedo");
			return action;
		}

		public void Undo()
		{
			if (this.position > 0)
			{
				--this.position;
				this.commands[this.position].Undo();
				if (this.position == 0)
				{
					this.RaisePropertyChanged("CanUndo");
				}
				if (this.position == this.commands.Count - 1)
				{
					this.RaisePropertyChanged("CanRedo");
				}
			}
		}

		public void Unlock()
		{
			--this.lockCounter;
		}

		#endregion

		#region Methods

		private void RaisePropertyChanged(string property)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(property));
			}
		}

		#endregion
	}
}