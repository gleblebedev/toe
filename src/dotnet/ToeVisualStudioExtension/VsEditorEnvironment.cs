using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Editors.Interfaces.Views;
using Toe.Editors.Marmalade;
using Toe.Resources;
using Toe.Utils.Mesh.Marmalade.IwGraphics;
using Toe.Utils.Mesh.Marmalade.IwGx;
using Toe.Utils.Mesh.Marmalade.IwResManager;

namespace TinyOpenEngine.ToeVisualStudioExtension
{
	public class VsEditorEnvironment : IEditorEnvironment
	{
		private readonly ToeVisualStudioExtensionPackage package;

		private readonly IResourceManager resourceManager;

		public VsEditorEnvironment(ToeVisualStudioExtensionPackage package, IResourceManager resourceManager)
		{
			this.package = package;
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
				return new ModelEditor(this, resourceManager);
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
			package.OpenFile(filePath);
		}

		#endregion
	}
}