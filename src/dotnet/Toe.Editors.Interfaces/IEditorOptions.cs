namespace Toe.Editors.Interfaces
{
	public interface IEditorOptions<T>
	{
		T Options { get; }

		void Load();

		void Save();
	}
}