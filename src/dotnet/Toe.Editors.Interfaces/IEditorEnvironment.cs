using Toe.Editors.Interfaces.Bindings;

namespace Toe.Editors.Interfaces
{
	public interface IEditorEnvironment
	{
		IView EditorFor(object itemToEdit);

		void Open(string filePath);
	}
}