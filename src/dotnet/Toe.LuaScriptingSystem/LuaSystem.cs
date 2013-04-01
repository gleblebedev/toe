using Toe.Core;

namespace Toe.LuaScriptingSystem
{
	public class LuaSystem : ISystem
	{
		#region Constants and Fields

		private readonly ToeSystemConfiguration configuration;

		private readonly ToeScene scene;

		#endregion

		#region Constructors and Destructors

		public LuaSystem(ToeScene scene, ToeSystemConfiguration configuration)
		{
			this.scene = scene;
			this.configuration = configuration;
		}

		#endregion
	}
}