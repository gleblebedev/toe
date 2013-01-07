using Autofac;

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
		}

		#endregion
	}
}