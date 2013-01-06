using Autofac;

using Toe.Marmalade.IwAnim;
using Toe.Marmalade.IwGx;
using Toe.Marmalade.IwResManager;
using Toe.Resources;
using Toe.Utils.Marmalade.IwGraphics;
using Toe.Utils.Marmalade.IwGx;

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

			builder.RegisterType<ManagedResourceType<Material>>().Keyed<IResourceType>(Material.TypeHash).SingleInstance();
			builder.RegisterType<ManagedResourceType<AnimSkin>>().Keyed<IResourceType>(AnimSkin.TypeHash).SingleInstance();
			builder.RegisterType<ManagedResourceType<AnimSkel>>().Keyed<IResourceType>(AnimSkel.TypeHash).SingleInstance();
			builder.RegisterType<ManagedResourceType<Anim>>().Keyed<IResourceType>(Anim.TypeHash).SingleInstance();
			builder.RegisterType<ManagedResourceType<Texture>>().Keyed<IResourceType>(Texture.TypeHash).SingleInstance();
			builder.RegisterType<ManagedResourceType<ResGroup>>().Keyed<IResourceType>(ResGroup.TypeHash).SingleInstance();
			builder.RegisterType<ManagedResourceType<Model>>().Keyed<IResourceType>(Model.TypeHash).SingleInstance();
			builder.RegisterType<ManagedResourceType<ShaderTechnique>>().Keyed<IResourceType>(ShaderTechnique.TypeHash).
				SingleInstance();
		}

		#endregion
	}
}