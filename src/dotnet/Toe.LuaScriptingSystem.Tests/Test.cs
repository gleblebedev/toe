using Autofac;

using NUnit.Framework;

using Toe.Core;

namespace Toe.LuaScriptingSystem.Tests
{
	[TestFixture]
	public class Test:BaseTest
	{
		[Test]
		public void Test1()
		{
			//    var toeMessageRegistry = new ToeMessageRegistry();
			//    var sc = new ToeMessageDescription("WebViewEvalJS", new[] { new FieldDescription("SourceCode", FieldType.String), });
			//    toeMessageRegistry.Register(ref sc);
			//    var toeSceneConfiguration = new ToeSceneConfiguration();
			//    toeSceneConfiguration.Systems.Add(new ToeSystemConfiguration(){});
			//    var scene = new ToeScene(toeSceneConfiguration, toeMessageRegistry);

			//    new LuaSystem()
			var lua = this.Container.ResolveKeyed<ISystem>("CtoeLuaSystem");
		}
	}
}