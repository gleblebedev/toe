using Autofac;

using Toe.Marmalade.IwAnim;
using Toe.Marmalade.IwGraphics;
using Toe.Marmalade.IwGx;
using Toe.Marmalade.IwResManager;
using Toe.Marmalade.TextFiles.IwAnim;
using Toe.Marmalade.TextFiles.IwGraphics;
using Toe.Marmalade.TextFiles.IwGx;
using Toe.Marmalade.TextFiles.IwResManager;
using Toe.Resources;
using Toe.Utils;

namespace Toe.Marmalade.TextFiles
{
	public class MarmaladeTextFilesAutofacModule : Module
	{
		#region Methods

		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);
			builder.RegisterType<MaterialTextSerializer>().Keyed<ITextSerializer>(Material.TypeHash).SingleInstance();
			builder.RegisterType<SkinTextSerializer>().Keyed<ITextSerializer>(AnimSkin.TypeHash).SingleInstance();
			builder.RegisterType<AnimTextSerializer>().Keyed<ITextSerializer>(Anim.TypeHash).SingleInstance();
			builder.RegisterType<SkelTextSerializer>().Keyed<ITextSerializer>(AnimSkel.TypeHash).SingleInstance();
			builder.RegisterType<ModelTextSerializer>().Keyed<ITextSerializer>(Model.TypeHash).Keyed<ITextSerializer>(
				Hash.Get("CMesh")).SingleInstance();
			builder.RegisterType<ShaderTextSerializer>().Keyed<ITextSerializer>(ShaderTechnique.TypeHash).SingleInstance();
			builder.RegisterType<GroupTextSerializer>().Keyed<ITextSerializer>(ResGroup.TypeHash).SingleInstance();

			builder.RegisterType<TextResourceFormat>().As<IResourceFileFormat>().SingleInstance();
		}

		#endregion
	}
}