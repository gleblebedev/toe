using Autofac;
using Autofac.Core;
using Autofac.Core.Activators.Reflection;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Editors.Interfaces.Views;
using Toe.Editors.Marmalade;
using Toe.Resources;
using Toe.Utils.Marmalade.IwGraphics;
using Toe.Utils.Marmalade.IwGx;
using Toe.Utils.Marmalade.IwResManager;

namespace Toe.Editor
{
	public class EditorEnvironment : IEditorEnvironment
	{
		private readonly MainEditorWindow mainEditorWindow;

		private readonly IResourceManager resourceManager;

		private readonly IComponentContext context;

		public EditorEnvironment(MainEditorWindow mainEditorWindow, IResourceManager resourceManager, IComponentContext context)
		{
			this.mainEditorWindow = mainEditorWindow;
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
			mainEditorWindow.OpenFile(filePath);
		}

		#endregion
	}
}