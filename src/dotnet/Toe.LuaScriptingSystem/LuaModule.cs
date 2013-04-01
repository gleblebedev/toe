using Autofac;

using Toe.Core;

namespace Toe.LuaScriptingSystem
{
	public class LuaModule: Autofac.Module
	{
		protected override void Load(Autofac.ContainerBuilder builder)
		{
			base.Load(builder);
			builder.RegisterType<LuaSystem>().Keyed<ISystem>("CtoeLuaSystem").InstancePerLifetimeScope();
		}
	}
}