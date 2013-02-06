using Autofac;

namespace Toe.Utils.Mesh.Bsp
{
	public class BspAutofacModule : Module
	{
		#region Methods

		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			builder.RegisterType<BspSceneFileFormat>().As<ISceneFileFormat>().InstancePerDependency();
		}

		#endregion
	}
}