namespace Toe.Editors.Interfaces
{
	public interface IResourceEditorFactory
	{
		#region Public Methods and Operators

		IResourceEditor CreateEditor(string filename);

		#endregion
	}
}