using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Editors.Interfaces.Views;
using Toe.Editors.Marmalade;
using Toe.Utils.Mesh.Marmalade.IwGx;
using Toe.Utils.Mesh.Marmalade.IwResManager;

namespace Toe.Editor
{
	public class EditorEnvironment : IEditorEnvironment
	{
		private readonly MainEditorWindow mainEditorWindow;

		public EditorEnvironment(MainEditorWindow mainEditorWindow)
		{
			this.mainEditorWindow = mainEditorWindow;
		}

		#region Implementation of IEditorEnvironment

		public IView EditorFor(object itemToEdit)
		{
			if (itemToEdit is Material)
			{
				return new MaterialEditor(this);
			}
			if (itemToEdit is ResGroup)
			{
				return new ResGroupEditor(this);
			}
			return new StringView();
		}

		#endregion
	}
}