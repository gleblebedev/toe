﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

using Autofac;

using Toe.Editors.Interfaces;
using Toe.Editors.Marmalade;
using Toe.Gx;
using Toe.Resources;
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