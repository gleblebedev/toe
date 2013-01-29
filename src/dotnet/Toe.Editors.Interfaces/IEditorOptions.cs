namespace Toe.Editors.Interfaces
{
	public interface IEditorOptions<T>
	{
		#region Public Properties

		T Options { get; }

		#endregion

		#region Public Methods and Operators

		void Save();

		#endregion
	}
}