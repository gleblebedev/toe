using Autofac;

using Toe.Marmalade.BinaryFiles.IwAnim;
using Toe.Marmalade.BinaryFiles.IwGraphics;
using Toe.Marmalade.BinaryFiles.IwGx;
using Toe.Marmalade.IwAnim;
using Toe.Marmalade.IwGx;
using Toe.Resources;
using Toe.Utils.Marmalade.IwGraphics;

namespace Toe.Marmalade.BinaryFiles
{
	public class MarmaladeBinaryFilesAutofacModule : Module
	{
		#region Methods

		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			builder.RegisterType<MaterialBinarySerializer>().Keyed<IBinarySerializer>(Material.TypeHash).SingleInstance();
			builder.RegisterType<ModelBinarySerializer>().Keyed<IBinarySerializer>(Model.TypeHash).SingleInstance();
			builder.RegisterType<TextureBinarySerializer>().Keyed<IBinarySerializer>(Texture.TypeHash).SingleInstance();
			builder.RegisterType<AnimBinarySerializer>().Keyed<IBinarySerializer>(Anim.TypeHash).SingleInstance();
			builder.RegisterType<SkelBinarySerializer>().Keyed<IBinarySerializer>(AnimSkel.TypeHash).SingleInstance();
			builder.RegisterType<SkinBinarySerializer>().Keyed<IBinarySerializer>(AnimSkin.TypeHash).SingleInstance();

			builder.RegisterType<BinaryResourceFormat>().As<IResourceFileFormat>().SingleInstance();
		}

		#endregion
	}
}