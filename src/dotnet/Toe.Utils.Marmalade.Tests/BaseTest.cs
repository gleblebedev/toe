using System;
using System.Collections.Generic;
using System.ComponentModel;

using Autofac;

using NUnit.Framework;

using Toe.Editors;
using Toe.Editors.Marmalade;
using Toe.Gx;
using Toe.Marmalade;
using Toe.Marmalade.BinaryFiles;
using Toe.Marmalade.TextFiles;
using Toe.Marmalade.TextureFiles;
using Toe.Resources;

using IContainer = Autofac.IContainer;
using ResourceFileItem = Toe.Marmalade.TextFiles.ResourceFileItem;

namespace Toe.Utils.Mesh.Marmalade.Tests
{
	public class BaseTest
	{
		#region Constants and Fields

		private ContainerBuilder builder;

		private IContainer container;

		#endregion

		#region Public Properties

		public IContainer Container
		{
			get
			{
				return this.container;
			}
		}

		#endregion

		#region Public Methods and Operators

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			this.builder = new ContainerBuilder();
			this.builder.RegisterGeneric(typeof(BindingList<>)).UsingConstructor(new Type[] { }).As(typeof(IList<>)).
				InstancePerDependency();
			this.builder.RegisterModule<BaseEditorsAutofacModule>();
			this.builder.RegisterModule<MarmaladeEditorsAutofacModule>();
			this.builder.RegisterModule<MarmaladeAutofacModule>();
			this.builder.RegisterModule<MarmaladeTextFilesAutofacModule>();
			this.builder.RegisterModule<MarmaladeBinaryFilesAutofacModule>();
			this.builder.RegisterModule<MarmaladeTextureFilesAutofacModule>();

			this.builder.RegisterType<ToeGraphicsContext>().As<ToeGraphicsContext>().SingleInstance();
			this.builder.RegisterType<ResourceErrorHandler>().As<IResourceErrorHandler>().SingleInstance();
			this.builder.RegisterType<ResourceManager>().As<IResourceManager>().SingleInstance();
			this.builder.RegisterType<ResourceFile>().As<IResourceFile>().InstancePerDependency();
			this.builder.RegisterType<ResourceFileItem>().As<IResourceFileItem>().InstancePerDependency();
			this.container = this.builder.Build();

			var l = this.container.Resolve<IList<int>>();
		}

		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			this.Container.Dispose();
		}

		#endregion
	}
}