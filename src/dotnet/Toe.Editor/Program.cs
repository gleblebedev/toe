using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

using Autofac;

using OpenTK;

using Toe.Editors.Interfaces;
using Toe.Editors.Marmalade;
using Toe.Gx;
using Toe.Resources;
using Toe.Utils.Mesh;
using Toe.Utils.Mesh.Marmalade;

using AutofacModule = Toe.Utils.Marmalade.AutofacModule;
using IContainer = Autofac.IContainer;

namespace Toe.Editor
{
	internal static class Program
	{
		private static IContainer container;

		#region Methods

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			var bip01 = new MeshBone { BindingPos = new Vector3(0, -32.7f, 530.3f), BindingRot = new Quaternion(0.70710726587626f, 0, 0, -0.707106225786538f) };
			var pelvis = new MeshBone { BindingPos = new Vector3(0, 0, 0), BindingRot = new Quaternion(0.5000006934f, -0.499999256601031f, - 0.5f, - 0.499999950000069f) };
			var spine = new MeshBone { BindingPos = new Vector3(56.4f, -4.6f, 0), BindingRot = new Quaternion(0.999568394358285f, -2.05893114629865E-06f, 0f, -0.0293766841426108f) };

			var v = bip01.BindingPos;
			var q = bip01.BindingRot;

			v = Vector3.Transform(pelvis.BindingPos, q)+v;
			//q = Quaternion.Multiply(q, pelvis.BindingRot);
			q = Quaternion.Multiply(pelvis.BindingRot, q);

			v = Vector3.Transform(spine.BindingPos, q) + v;
			q = Quaternion.Multiply(spine.BindingRot, q);

			AppDomain.CurrentDomain.UnhandledException += OnException;
			var cb = new Autofac.ContainerBuilder();

			cb.RegisterModule<Toe.Editors.Marmalade.AutofacModule>();
			cb.RegisterModule<AutofacModule>();
			cb.RegisterGeneric(typeof(BindingList<>)).UsingConstructor(new Type[] { }).As(typeof(IList<>));
			cb.RegisterType<EditorEnvironment>().As<IEditorEnvironment>().SingleInstance();
			cb.RegisterType<ResourceManager>().As<IResourceManager>().SingleInstance();
			cb.RegisterType<ResourceFile>().As<IResourceFile>().InstancePerDependency();
			cb.RegisterType<ResourceFileItem>().As<IResourceFileItem>().InstancePerDependency();
			cb.RegisterType<ResourceEditorFactory>().As<IResourceEditorFactory>().SingleInstance();
			cb.RegisterType<EditorResourceErrorHandler>().As<IResourceErrorHandler>().SingleInstance();
			cb.RegisterType<ToeGraphicsContext>().As<ToeGraphicsContext>().SingleInstance();
			
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