using System;
using System.Collections.Generic;
using System.ComponentModel;

using Autofac;

using NUnit.Framework;

using Toe.Editors.Marmalade;
using Toe.Gx;
using Toe.Resources;
using Toe.Utils.Marmalade;

using AutofacModule = Toe.Utils.Marmalade.AutofacModule;
using IContainer = Autofac.IContainer;

namespace Toe.Utils.Mesh.Marmalade.Tests
{
	public class BaseTest
	{
		private ContainerBuilder builder;

		private IContainer container;

		public IContainer Container
		{
			get
			{
				return this.container;
			}
		}

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			this.builder = new Autofac.ContainerBuilder();
			this.builder.RegisterGeneric(typeof(BindingList<>)).UsingConstructor(new Type[]{}).As(typeof(IList<>)).InstancePerDependency(); ;
			this.builder.RegisterModule<Toe.Editors.Marmalade.AutofacModule>();
			this.builder.RegisterModule<AutofacModule>();
			this.builder.RegisterType<ToeGraphicsContext>().As<ToeGraphicsContext>().SingleInstance();
			this.builder.RegisterType<ResourceErrorHandler>().As<IResourceErrorHandler>().SingleInstance();
			this.builder.RegisterType<ResourceManager>().As<IResourceManager>().SingleInstance();
			this.builder.RegisterType<ResourceFile>().As<IResourceFile>().InstancePerDependency();
			this.builder.RegisterType<ResourceFileItem>().As<IResourceFileItem>().InstancePerDependency();
			this.container = this.builder.Build();

			var l = container.Resolve<IList<int>>();
		}
		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			this.Container.Dispose();
		}
	}
}