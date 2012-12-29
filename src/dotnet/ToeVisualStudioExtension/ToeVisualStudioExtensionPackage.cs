using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

using Autofac;

using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

using Toe.Editors.Geometry;
using Toe.Editors.Interfaces;
using Toe.Editors.Marmalade;
using Toe.Gx;
using Toe.Resources;
using Toe.ToeVsExt;
using Toe.Utils.Mesh.Marmalade;
using Toe.Utils.Mesh.Marmalade.IwGraphics;

using IContainer = Autofac.IContainer;

namespace TinyOpenEngine.ToeVisualStudioExtension
{
	/// <summary>
	/// This is the class that implements the package exposed by this assembly.
	///
	/// The minimum requirement for a class to be considered a valid package for Visual Studio
	/// is to implement the IVsPackage interface and register itself with the shell.
	/// This package uses the helper classes defined inside the Managed Package Framework (MPF)
	/// to do it: it derives from the Package class that provides the implementation of the 
	/// IVsPackage interface and uses the registration attributes defined in the framework to 
	/// register itself and its components with the shell.
	/// </summary>
	// This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
	// a package.
	[PackageRegistration(UseManagedResourcesOnly = true)]
	// This attribute is used to register the informations needed to show the this package
	// in the Help/About dialog of Visual Studio.
	[InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
	// This attribute is needed to let the shell know that this package exposes some menus.
	[ProvideMenuResource("Menus.ctmenu", 1)]
	[ProvideEditorExtension(typeof(EditorFactory), ".group", 50, ProjectGuid = "{A2FE74E1-B743-11d0-AE1A-00A0C90FFFC3}",
		TemplateDir = "Templates", NameResourceID = 105, DefaultName = "Group")]
	[ProvideEditorExtension(typeof(EditorFactory), ".geo", 50, ProjectGuid = "{A2FE74E1-B743-11d0-AE1A-00A0C90FFFC3}",
		TemplateDir = "Templates", NameResourceID = 105, DefaultName = "Geometry")]
	[ProvideEditorExtension(typeof(EditorFactory), ".anim", 50, ProjectGuid = "{A2FE74E1-B743-11d0-AE1A-00A0C90FFFC3}",
		TemplateDir = "Templates", NameResourceID = 105, DefaultName = "Animation")]
	[ProvideEditorExtension(typeof(EditorFactory), ".skin", 50, ProjectGuid = "{A2FE74E1-B743-11d0-AE1A-00A0C90FFFC3}",
		TemplateDir = "Templates", NameResourceID = 105, DefaultName = "Skin")]
	[ProvideEditorExtension(typeof(EditorFactory), ".skel", 50, ProjectGuid = "{A2FE74E1-B743-11d0-AE1A-00A0C90FFFC3}",
		TemplateDir = "Templates", NameResourceID = 105, DefaultName = "Skeleton")]
	[ProvideEditorExtension(typeof(EditorFactory), ".mtl", 50, ProjectGuid = "{A2FE74E1-B743-11d0-AE1A-00A0C90FFFC3}",
		TemplateDir = "Templates", NameResourceID = 105, DefaultName = "Material")]
	[ProvideEditorExtension(typeof(EditorFactory), ".tga", 50, ProjectGuid = "{A2FE74E1-B743-11d0-AE1A-00A0C90FFFC3}",
		TemplateDir = "Templates", NameResourceID = 105, DefaultName = "Material")]
	[ProvideEditorExtension(typeof(EditorFactory), ".bmp", 50, ProjectGuid = "{A2FE74E1-B743-11d0-AE1A-00A0C90FFFC3}",
		TemplateDir = "Templates", NameResourceID = 105, DefaultName = "Material")]
	[ProvideEditorExtension(typeof(EditorFactory), ".bin", 1000, ProjectGuid = "{A2FE74E1-B743-11d0-AE1A-00A0C90FFFC3}",
		NameResourceID = 105)]
	[ProvideKeyBindingTable(GuidList.guidToeVisualStudioExtensionEditorFactoryString, 102)]
	[ProvideEditorLogicalView(typeof(EditorFactory), "{7651a703-06e5-11d1-8ebd-00a0c90f26ea}")]
	[Guid(GuidList.guidToeVisualStudioExtensionPkgString)]
	public sealed class ToeVisualStudioExtensionPackage : Package
	{
		#region Constructors and Destructors

		/// <summary>
		/// Default constructor of the package.
		/// Inside this method you can place any initialization code that does not require 
		/// any Visual Studio service because at this point the package object is created but 
		/// not sited yet inside Visual Studio environment. The place to do all the other 
		/// initialization is the Initialize method.
		/// </summary>
		public ToeVisualStudioExtensionPackage()
		{
			Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this));
		}

		#endregion

		/////////////////////////////////////////////////////////////////////////////
		// Overriden Package Implementation

		#region Methods

		/// <summary>
		/// Initialization of the package; this method is called right after the package is sited, so this is the place
		/// where you can put all the initilaization code that rely on services provided by VisualStudio.
		/// </summary>
		protected override void Initialize()
		{
			Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this));
			base.Initialize();

			var cb = new Autofac.ContainerBuilder();

			cb.RegisterModule<AutofacModule>();
			cb.RegisterGeneric(typeof(BindingList<>)).UsingConstructor(new Type[] { }).As(typeof(IList<>));
			cb.RegisterType<VsEditorEnvironment>().As<IEditorEnvironment>().SingleInstance();
			cb.RegisterType<ResourceManager>().As<IResourceManager>().SingleInstance();
			cb.RegisterType<ResourceFile>().As<IResourceFile>().InstancePerDependency();
			cb.RegisterType<ResourceFileItem>().As<IResourceFileItem>().InstancePerDependency();
			cb.RegisterType<ResourceEditorFactory>().As<IResourceEditorFactory>().SingleInstance();
			cb.RegisterType<TextResourceFormat>().As<IResourceFileFormat>().SingleInstance();
			cb.RegisterType<TextureResourceFormat>().As<IResourceFileFormat>().SingleInstance();
			cb.RegisterType<EditorResourceErrorHandler>().As<IResourceErrorHandler>().SingleInstance();
			cb.RegisterType<ToeGrapicsContext>().As<ToeGrapicsContext>().SingleInstance();
			cb.RegisterType<EditorFactory>().As<IVsEditorFactory>().SingleInstance();
			cb.RegisterInstance(this).As<Package>().As<ToeVisualStudioExtensionPackage>().ExternallyOwned();

			container = cb.Build();

			foreach (var f in container.Resolve<IEnumerable<IVsEditorFactory>>())
			{
				//Create Editor Factory. Note that the base Package class will call Dispose on it.
				base.RegisterEditorFactory(f);
			}
		}

		#endregion

		private static IContainer container;

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				if (container != null)
				{
					container.Dispose();
					container = null;
				}
			}
		}

		//public IEditorEnvironment CreateEditorEnvironmentProxy()
		//{
		//    return new VsEditorEnvironment(this, container.Resolve<IResourceManager>());
		//}

		public void OpenFile(string filePath)
		{
			EnvDTE80.DTE2 dte = (EnvDTE80.DTE2)this.GetService(typeof(SDTE));
			// http://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.logicalview.aspx

			var primary = "{00000000-0000-0000-0000-000000000000}";
			var any = "{FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF}";
			var window = dte.OpenFile(primary, filePath);
			window.Activate();
		}
	}
}