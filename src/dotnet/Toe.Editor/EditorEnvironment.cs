using Autofac;
using Autofac.Core;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Editors.Interfaces.Views;
using Toe.Resources;

namespace Toe.Editor
{
	public class EditorEnvironment : IEditorEnvironment
	{
		#region Constants and Fields

		private readonly IComponentContext context;

		private readonly MainEditorWindow mainEditorWindow;

		private readonly IResourceManager resourceManager;

		#endregion

		#region Constructors and Destructors

		public EditorEnvironment(
			MainEditorWindow mainEditorWindow, IResourceManager resourceManager, IComponentContext context)
		{
			this.mainEditorWindow = mainEditorWindow;
			this.resourceManager = resourceManager;
			this.context = context;
		}

		#endregion

		#region Public Methods and Operators

		public IView EditorFor(object itemToEdit, ICommandHistory history)
		{
			var typedParameters = new Parameter[] { TypedParameter.From(history) };
			IView view = this.context.ResolveOptionalKeyed<IView>(itemToEdit.GetType(), typedParameters);
			if (view != null)
			{
				return view;
			}
			view = this.context.ResolveOptionalKeyed<IView>(itemToEdit.GetType().BaseType, typedParameters);
			if (view != null)
			{
				return view;
			}
			return new StringView();
		}

		public void Open(string filePath)
		{
			this.mainEditorWindow.OpenFile(filePath);
		}

		#endregion
	}
}