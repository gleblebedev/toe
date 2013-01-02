using Autofac;

using Toe.Marmalade.IwAnim;
using Toe.Marmalade.IwGx;
using Toe.Utils.Marmalade.IwAnim;
using Toe.Utils.Marmalade.IwGraphics;
using Toe.Utils.Marmalade.IwGx;
using Toe.Utils.Marmalade.IwResManager;

namespace Toe.Marmalade
{
	public class MarmaladeAutofacModule : Module
	{
		#region Methods

		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);
			builder.RegisterType<Material>().As<Material>();
			builder.RegisterType<AnimSkin>().As<AnimSkin>();
			builder.RegisterType<AnimSkel>().As<AnimSkel>();
			builder.RegisterType<Anim>().As<Anim>();
			builder.RegisterType<Texture>().As<Texture>();
			builder.RegisterType<ResGroup>().As<ResGroup>();
			builder.RegisterType<Model>().As<Model>();
			builder.RegisterType<ShaderTechnique>().As<ShaderTechnique>();

		
		}

		#endregion
	}
}