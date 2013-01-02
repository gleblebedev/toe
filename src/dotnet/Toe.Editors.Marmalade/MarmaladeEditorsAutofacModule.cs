using Autofac;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Editors.Interfaces.Views;
using Toe.Marmalade.IwAnim;
using Toe.Marmalade.IwGx;
using Toe.Resources;
using Toe.Utils.Marmalade.IwAnim;
using Toe.Utils.Marmalade.IwGraphics;
using Toe.Utils.Marmalade.IwGx;
using Toe.Utils.Marmalade.IwResManager;

namespace Toe.Editors.Marmalade
{
	public class MarmaladeEditorsAutofacModule : Module
	{
		#region Methods

		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);
			builder.RegisterType<MaterialEditor>().Keyed<IView>(typeof(Material)).InstancePerDependency();
			builder.RegisterType<ResGroupEditor>().Keyed<IView>(typeof(ResGroup)).InstancePerDependency();
			builder.RegisterType<ModelEditor>().Keyed<IView>(typeof(Model)).InstancePerDependency();
			builder.RegisterType<AnimEditor>().Keyed<IView>(typeof(Anim)).InstancePerDependency();
			builder.RegisterType<SkeletonEditor>().Keyed<IView>(typeof(AnimSkel)).InstancePerDependency();
			builder.RegisterType<SkinEditor>().Keyed<IView>(typeof(AnimSkin)).InstancePerDependency();
			builder.RegisterType<TextureEditor>().Keyed<IView>(typeof(Texture)).InstancePerDependency();
			builder.RegisterType<ShaderEditor>().Keyed<IView>(typeof(ShaderTechnique)).InstancePerDependency();
			builder.RegisterType<ResourceFileReferenceEditor>().Keyed<IView>(typeof(ResourceFile)).InstancePerDependency();

			builder.RegisterType<ResourceFileEditor>().As<ResourceFileEditor>().InstancePerDependency();
			builder.RegisterType<ResourceEditorFactory>().As<IResourceEditorFactory>().SingleInstance();
		}

		#endregion
	}
}