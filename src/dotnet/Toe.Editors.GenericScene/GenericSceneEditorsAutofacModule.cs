using Autofac;

using Toe.Editors.Interfaces;

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