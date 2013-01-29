using System;

using Autofac;

using Toe.Editors.Interfaces;

namespace Toe.Editors.GenericScene
{
	public class GenericSceneResourceEditorFactory : IResourceEditorFactory
	{
		#region Constants and Fields

		private readonly IComponentContext context;

		#endregion

		#region Constructors and Destructors

		public GenericSceneResourceEditorFactory(IComponentContext context)
		{
			this.context = context;
		}

		#endregion

		#region Public Methods and Operators

		public IResourceEditor CreateEditor(string fileName)
		{
			if (fileName.EndsWith(".ase", StringComparison.InvariantCultureIgnoreCase)
			    || fileName.EndsWith(".dae", StringComparison.InvariantCultureIgnoreCase))
			{
				return this.context.Resolve<GenericSceneEditor>();
			}
			return null;
		}

		#endregion
	}
}