using Autofac;

using Toe.Editors.Interfaces;

namespace Toe.Editors
{
	public class BaseEditorsAutofacModule : Module
	{
		#region Methods

		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);
			builder.RegisterType<Base3DEditor>().InstancePerDependency();
			builder.RegisterType<Base3DEditorContent>().SingleInstance();
			builder.RegisterGeneric(typeof(DefaultEditorConfiguration<>)).As(typeof(IEditorOptions<>)).SingleInstance();
			builder.RegisterType<DefaultEditorConfigStorage>().As<IEditorConfigStorage>().SingleInstance();
			builder.RegisterType<ImportSceneDialog>().As<ImportSceneDialog>().SingleInstance();
		}

		#endregion
	}
}