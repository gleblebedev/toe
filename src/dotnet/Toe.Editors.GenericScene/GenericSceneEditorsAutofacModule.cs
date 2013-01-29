using Autofac;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Editors.Interfaces.Views;

namespace Toe.Editors.GenericScene
{
	public class GenericSceneEditorsAutofacModule : Module
	{
		#region Methods

		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);
			builder.RegisterType<GenericSceneEditor>().As<GenericSceneEditor>().InstancePerDependency();
			builder.RegisterType<GenericSceneResourceEditorFactory>().As<IResourceEditorFactory>().SingleInstance();
		}

		#endregion
	}
}