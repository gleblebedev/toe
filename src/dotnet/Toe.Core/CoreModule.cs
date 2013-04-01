using Autofac;

namespace Toe.Core
{
	public class CoreModule : Autofac.Module
	{
		protected override void Load(Autofac.ContainerBuilder builder)
		{
			base.Load(builder);
			builder.RegisterType<ToeMessageRegistry>().As<ToeMessageRegistry>().SingleInstance();
		}
	}
}