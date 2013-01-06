using System.ComponentModel;

namespace Toe.Editors.Interfaces
{
	public interface ICommandHistory : INotifyPropertyChanged
	{
		#region Public Properties

		bool CanRedo { get; }

		bool CanUndo { get; }

		bool IsLocked { get; }

		ICommand Top { get; }

		#endregion

		#region Public Methods and Operators

		void Clear();

		void DropRedo();

		void Lock();

		void Redo();

		ICommand RegisterAction(ICommand action);

		void Undo();

		void Unlock();

		#endregion
	}
}