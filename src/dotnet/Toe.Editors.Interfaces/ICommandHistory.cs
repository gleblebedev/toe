using System.ComponentModel;

namespace Toe.Editors.Interfaces
{
	public interface ICommandHistory: INotifyPropertyChanged
	{
		void Redo();

		void Undo();

		bool CanRedo { get; }

		bool CanUndo { get; }

		ICommand RegisterAction(ICommand action);

		void Clear();

		void Lock();

		void Unlock();

		bool IsLocked { get; }

		ICommand Top { get; }

		void DropRedo();
	}
}