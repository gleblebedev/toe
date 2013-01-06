namespace Toe.Editors.Interfaces
{
	public interface ICommand
	{
		#region Public Methods and Operators

		void Redo();

		void Undo();

		#endregion
	}
}