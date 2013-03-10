using Autofac;

namespace Toe.Utils.Mesh.Svg
{
	public class SvgAutofacModule : Module
	{
		#region Methods

		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			builder.RegisterType<SvgSceneFileFormat>().As<ISceneFileFormat>().InstancePerDependency();
		}

		#endregion
	}
}