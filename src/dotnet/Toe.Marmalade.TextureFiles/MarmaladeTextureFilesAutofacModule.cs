using Autofac;

using Toe.Resources;

namespace Toe.Marmalade.TextureFiles
{
	public class MarmaladeTextureFilesAutofacModule : Module
	{
		#region Methods

		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			builder.RegisterType<TextureResourceFormat>().As<IResourceFileFormat>().SingleInstance();
		}

		#endregion
	}
}