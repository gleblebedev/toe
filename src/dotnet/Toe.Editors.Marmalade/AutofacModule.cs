using System;

using Autofac;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Utils.Mesh.Marmalade.IwGx;
using Toe.Utils.Mesh.Marmalade.IwResManager;

namespace Toe.Editors.Marmalade
{
	public class AutofacModule: Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			builder.RegisterType<MaterialEditor>().As<IView>().Keyed<object>(typeof(Material)).InstancePerDependency();
			builder.RegisterType<ResGroupEditor>().As<IView>().Keyed<object>(typeof(ResGroup)).InstancePerDependency();
			builder.RegisterType<ResourceFileEditor>().As<ResourceFileEditor>().InstancePerDependency();
			builder.RegisterType<ResourceEditorFactory>().As<IResourceEditorFactory>().SingleInstance();
		}
	}
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
				fileName.EndsWith(".group", StringComparison.InvariantCultureIgnoreCase))
			{
				return context.Resolve<ResourceFileEditor>();
			}
			return null;
		}

		#endregion
	}
}