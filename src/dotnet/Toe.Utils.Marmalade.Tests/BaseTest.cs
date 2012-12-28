using System;
using System.Collections.Generic;
using System.ComponentModel;

using Autofac;

using NUnit.Framework;

using Toe.Resources;

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
			this.builder.RegisterType<ResourceManager>().As<IResourceManager>().SingleInstance();
			this.builder.RegisterType<ResourceFile>().As<IResourceFile>().InstancePerDependency();
			this.builder.RegisterType<ResourceFileItem>().As<IResourceFileItem>().InstancePerDependency();
			this.builder.RegisterType<TextureResourceFormat>().As<IResourceFileFormat>().SingleInstance();
			this.builder.RegisterType<TextResourceFormat>().As<IResourceFileFormat>().SingleInstance();
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