using Autofac;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Resources;
using Toe.Utils.Mesh.Marmalade;
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
}