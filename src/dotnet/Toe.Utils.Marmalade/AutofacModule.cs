using Autofac;

using Toe.Resources;
using Toe.Utils.Marmalade.IwAnim;
using Toe.Utils.Marmalade.IwGraphics;
using Toe.Utils.Marmalade.IwGx;
using Toe.Utils.Marmalade.IwResManager;

namespace Toe.Utils.Marmalade
{
	public class AutofacModule : Module
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

			builder.RegisterType<MaterialTextSerializer>().Keyed<ITextSerializer>(Material.TypeHash).SingleInstance();
			builder.RegisterType<SkinTextSerializer>().Keyed<ITextSerializer>(AnimSkin.TypeHash).SingleInstance();
			builder.RegisterType<AnimTextSerializer>().Keyed<ITextSerializer>(Anim.TypeHash).SingleInstance();
			builder.RegisterType<SkelTextSerializer>().Keyed<ITextSerializer>(AnimSkel.TypeHash).SingleInstance();
			builder.RegisterType<ModelTextSerializer>().Keyed<ITextSerializer>(Model.TypeHash).Keyed<ITextSerializer>(Hash.Get("CMesh"))
				.SingleInstance();
			builder.RegisterType<ShaderTextSerializer>().Keyed<ITextSerializer>(ShaderTechnique.TypeHash).SingleInstance();
			builder.RegisterType<GroupTextSerializer>().Keyed<ITextSerializer>(ResGroup.TypeHash).SingleInstance();

			builder.RegisterType<BinaryResourceFormat>().As<IResourceFileFormat>().SingleInstance();
			builder.RegisterType<TextResourceFormat>().As<IResourceFileFormat>().SingleInstance();
			builder.RegisterType<TextureResourceFormat>().As<IResourceFileFormat>().SingleInstance();
		}

		#endregion
	}
}