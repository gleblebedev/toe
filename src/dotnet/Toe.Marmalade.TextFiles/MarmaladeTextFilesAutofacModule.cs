using System;

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
using Toe.Utils.Marmalade;

namespace Toe.Marmalade.TextFiles
{
	public class MarmaladeTextFilesAutofacModule : Module
	{
		#region Methods

		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);
			builder.RegisterType<MaterialTextDeserializer>().Keyed<ITextDeserializer>(Material.TypeHash).SingleInstance();
			builder.RegisterType<SkinTextDeserializer>().Keyed<ITextDeserializer>(AnimSkin.TypeHash).SingleInstance();
			builder.RegisterType<AnimTextDeserializer>().Keyed<ITextDeserializer>(Anim.TypeHash).SingleInstance();
			builder.RegisterType<SkelTextDeserializer>().Keyed<ITextDeserializer>(AnimSkel.TypeHash).SingleInstance();
			builder.RegisterType<ModelTextDeserializer>().Keyed<ITextDeserializer>(Model.TypeHash).Keyed<ITextDeserializer>(Hash.Get("CMesh")).SingleInstance();
			builder.RegisterType<ModelTextSerializer>().Keyed<ITextSerializer>(Model.TypeHash).Keyed<ITextSerializer>(Hash.Get("CMesh")).SingleInstance();
			builder.RegisterType<ShaderTextDeserializer>().Keyed<ITextDeserializer>(ShaderTechnique.TypeHash).SingleInstance();
			builder.RegisterType<GroupTextDeserializer>().Keyed<ITextDeserializer>(ResGroup.TypeHash).SingleInstance();

			builder.RegisterType<TextResourceFormat>().As<IResourceFileFormat>().SingleInstance();
			builder.RegisterType<TextResourceWriter>().As<TextResourceWriter>().SingleInstance();

			builder.Register<Func<uint, ITextSerializer>>(c =>
				{
					var componentContext = c.Resolve<IComponentContext>();
					return s =>
						{
							object v;
							if (componentContext.TryResolveKeyed(s, typeof(ITextSerializer), out v))
							{
								return (ITextSerializer)v;
							}
							return null;
						};
				});
			builder.Register<Func<uint, ITextDeserializer>>(c =>
				{
					var componentContext = c.Resolve<IComponentContext>();
					return s =>
						{
							object v;
							if (componentContext.TryResolveKeyed(s, typeof(ITextDeserializer), out v))
							{
								return (ITextDeserializer)v;
							}
							return null;
						};
				});
		}

		#endregion
	}
}