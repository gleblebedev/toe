using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

using Autofac;

using OpenTK;

using Toe.Editors;
using Toe.Editors.GenericScene;
using Toe.Editors.Interfaces;
using Toe.Editors.Marmalade;
using Toe.Gx;
using Toe.Marmalade;
using Toe.Marmalade.BinaryFiles;
using Toe.Marmalade.TextFiles;
using Toe.Marmalade.TextureFiles;
using Toe.Resources;
using Toe.Utils.Mesh.Ase;
using Toe.Utils.Mesh.Bsp;
using Toe.Utils.Mesh.Dae;
using Toe.Utils.Mesh.Svg;

using IContainer = Autofac.IContainer;
using ResourceFileItem = Toe.Marmalade.TextFiles.ResourceFileItem;

namespace Toe.Editor
{
	internal static class Program
	{
		#region Constants and Fields

		private static IContainer container;

		#endregion

		#region Methods

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			AppDomain.CurrentDomain.UnhandledException += OnException;
			var cb = new ContainerBuilder();
			cb.RegisterModule<BaseEditorsAutofacModule>();
			cb.RegisterModule<MarmaladeEditorsAutofacModule>();
			cb.RegisterModule<GenericSceneEditorsAutofacModule>();
			cb.RegisterModule<AseAutofacModule>();
			cb.RegisterModule<DaeAutofacModule>();
			cb.RegisterModule<BspAutofacModule>();
			cb.RegisterModule<SvgAutofacModule>();
			
			cb.RegisterModule<MarmaladeAutofacModule>();
			cb.RegisterModule<MarmaladeTextFilesAutofacModule>();
			cb.RegisterModule<MarmaladeBinaryFilesAutofacModule>();
			cb.RegisterModule<MarmaladeTextureFilesAutofacModule>();

			cb.RegisterGeneric(typeof(BindingList<>)).UsingConstructor(new Type[] { }).As(typeof(IList<>));
			//cb.RegisterGeneric(typeof(EditorConfiguration<>)).As(typeof(IEditorOptions<>)).SingleInstance();
			cb.RegisterType<EditorEnvironment>().As<IEditorEnvironment>().SingleInstance();
			cb.RegisterType<ResourceManager>().As<IResourceManager>().SingleInstance();
			cb.RegisterType<ResourceFile>().As<IResourceFile>().InstancePerDependency();
			cb.RegisterType<ResourceFileItem>().As<IResourceFileItem>().InstancePerDependency();
			cb.RegisterType<EditorResourceErrorHandler>().As<IResourceErrorHandler>().SingleInstance();
			cb.RegisterType<ToeGraphicsContext>().As<ToeGraphicsContext>().SingleInstance();


			cb.RegisterType<AddNewItemForm>().InstancePerDependency();
			cb.RegisterType<MainEditorWindow>().SingleInstance();

			using (container = cb.Build())
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(container.Resolve<MainEditorWindow>());
			}
		}

		private static void OnException(object sender, UnhandledExceptionEventArgs e)
		{
			Trace.WriteLine(e.ExceptionObject);
		}

		#endregion
	}
}