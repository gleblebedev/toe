using Autofac;

using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Editors.Interfaces.Views;
using Toe.Editors.Marmalade;
using Toe.Resources;
using Toe.Utils.Marmalade.IwAnim;
using Toe.Utils.Marmalade.IwGraphics;
using Toe.Utils.Marmalade.IwGx;
using Toe.Utils.Marmalade.IwResManager;

namespace TinyOpenEngine.ToeVisualStudioExtension
{
	public class VsEditorEnvironment : IEditorEnvironment
	{
		private readonly ToeVisualStudioExtensionPackage package;

		private readonly IResourceManager resourceManager;

		private readonly IComponentContext context;

		public VsEditorEnvironment(ToeVisualStudioExtensionPackage package, IResourceManager resourceManager, IComponentContext context)
		{
			this.package = package;
			this.resourceManager = resourceManager;
			this.context = context;
		}

		#region Implementation of IEditorEnvironment

		public IView EditorFor(object itemToEdit)
		{
			object view;
			if (context.TryResolveKeyed(itemToEdit.GetType(), typeof(IView), out view))
			{
				return (IView)view;
			}
			if (context.TryResolveKeyed(itemToEdit.GetType().BaseType, typeof(IView), out view))
			{
				return (IView)view;
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