using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Editors.Interfaces.Views;
using Toe.Editors.Marmalade;
using Toe.Resources;
using Toe.Utils.Marmalade.IwAnim;
using Toe.Utils.Marmalade.IwGraphics;
using Toe.Utils.Mesh.Marmalade.IwGraphics;
using Toe.Utils.Mesh.Marmalade.IwGx;
using Toe.Utils.Mesh.Marmalade.IwResManager;

namespace Toe.Editor
{
	public class EditorEnvironment : IEditorEnvironment
	{
		private readonly MainEditorWindow mainEditorWindow;

		private readonly IResourceManager resourceManager;

		public EditorEnvironment(MainEditorWindow mainEditorWindow, IResourceManager resourceManager)
		{
			this.mainEditorWindow = mainEditorWindow;
			this.resourceManager = resourceManager;
		}

		#region Implementation of IEditorEnvironment

		public IView EditorFor(object itemToEdit)
		{
			if (itemToEdit is Material)
			{
				return new MaterialEditor(this);
			}
			if (itemToEdit is Model)
			{
				return new ModelEditor(this,resourceManager);
			}
			if (itemToEdit is AnimSkel)
			{
				return new SkeletonEditor(this, resourceManager);
			}
			if (itemToEdit is AnimSkin)
			{
				return new SkinEditor(this, resourceManager);
			}
			if (itemToEdit is Anim)
			{
				return new AnimEditor(this, resourceManager);
			}
			if (itemToEdit is Texture)
			{
				return new TextureEditor(this, resourceManager);
			}
			if (itemToEdit is ResGroup)
			{
				return new ResGroupEditor(this);
			}
			if (itemToEdit is IResourceFile)
			{
				return new ResourceFileReferenceEditor(this);
			}
			return new StringView();
		}

		public void Open(string filePath)
		{
			mainEditorWindow.OpenFile(filePath);
		}

		#endregion
	}
}