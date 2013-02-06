using Autofac;

namespace Toe.Utils.Mesh.Ase
{
	public class AseAutofacModule : Module
	{
		#region Methods

		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			builder.RegisterType<AseSceneFileFormat>().As<ISceneFileFormat>().InstancePerDependency();
		}

		#endregion
	}
}