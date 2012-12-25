using System;
using System.Windows.Forms;

using Autofac;

using Toe.Editors.Interfaces;
using Toe.Editors.Marmalade;

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
			var cb = new Autofac.ContainerBuilder();

			cb.RegisterModule<AutofacModule>();
			cb.RegisterType<EditorEnvironment>().As<IEditorEnvironment>().SingleInstance();
			cb.RegisterType<MainEditorWindow>().SingleInstance();

			using (container = cb.Build())
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(container.Resolve<MainEditorWindow>());
			}
		}

		#endregion
	}
}