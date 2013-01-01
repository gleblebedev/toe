namespace Toe.Editors.Interfaces
{
	public interface ICommand
	{
		void Redo();

		void Undo();
	}
}