using Autofac;

using NUnit.Framework;

using Toe.Core;

namespace Toe.LuaScriptingSystem.Tests
{
	public class BaseTest
	{
		private Autofac.IContainer container;

		public IContainer Container
		{
			get
			{
				return this.container;
			}
		}

		[SetUp]
		public void SetUp()
		{
			var builder = new ContainerBuilder();
			builder.RegisterModule<CoreModule>();
			builder.RegisterModule<LuaModule>();
			this.container = builder.Build();
		}
		[TearDown]
		public void TearDown()
		{
			this.Container.Dispose();
			this.container = null;
		}
	}
}