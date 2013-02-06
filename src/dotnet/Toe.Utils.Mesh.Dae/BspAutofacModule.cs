using Autofac;

namespace Toe.Utils.Mesh.Dae
{
	public class DaeAutofacModule : Module
	{
		#region Methods

		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			builder.RegisterType<DaeSceneFileFormat>().As<ISceneFileFormat>().InstancePerDependency();
		}

		#endregion
	}
}