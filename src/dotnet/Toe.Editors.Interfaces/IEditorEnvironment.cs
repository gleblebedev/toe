using Toe.Editors.Interfaces.Bindings;

namespace Toe.Editors.Interfaces
{
	public interface IEditorEnvironment
	{
		#region Public Methods and Operators

		IView EditorFor(object itemToEdit, ICommandHistory history);

		void Open(string filePath);

		#endregion
	}
}