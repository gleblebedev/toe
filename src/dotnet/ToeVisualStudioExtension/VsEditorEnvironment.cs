using Autofac;
using Autofac.Core;

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

		public IView EditorFor(object itemToEdit, ICommandHistory history)
		{
			var typedParameters = new Parameter[] { TypedParameter.From(history) };
			IView view = context.ResolveOptionalKeyed<IView>(itemToEdit.GetType(), typedParameters);
			if (view != null)
			{
				return (IView)view;
			}
			view = context.ResolveOptionalKeyed<IView>(itemToEdit.GetType().BaseType, typedParameters);
			if (view != null)
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