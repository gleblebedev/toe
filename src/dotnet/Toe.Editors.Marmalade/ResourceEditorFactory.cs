using System;

using Autofac;

using Toe.Editors.Interfaces;

namespace Toe.Editors.Marmalade
{
	public class ResourceEditorFactory:IResourceEditorFactory
	{
		private readonly IComponentContext context;

		public ResourceEditorFactory(IComponentContext context)
		{
			this.context = context;
		}

		#region Implementation of IResourceEditorFactory

		public IResourceEditor CreateEditor(string fileName)
		{
			if (fileName.EndsWith(".mtl", StringComparison.InvariantCultureIgnoreCase) ||
			    fileName.EndsWith(".geo", StringComparison.InvariantCultureIgnoreCase) ||
			    fileName.EndsWith(".skin", StringComparison.InvariantCultureIgnoreCase) ||
			    fileName.EndsWith(".skel", StringComparison.InvariantCultureIgnoreCase) ||
			    fileName.EndsWith(".anim", StringComparison.InvariantCultureIgnoreCase) ||
			    fileName.EndsWith(".itx", StringComparison.InvariantCultureIgnoreCase) ||
			    fileName.EndsWith(".bmp", StringComparison.InvariantCultureIgnoreCase) ||
			    fileName.EndsWith(".tga", StringComparison.InvariantCultureIgnoreCase) ||
			    fileName.EndsWith(".group", StringComparison.InvariantCultureIgnoreCase))
			{
				return this.context.Resolve<ResourceFileEditor>();
			}
			return null;
		}

		#endregion
	}
}