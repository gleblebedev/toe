using Toe.Core;

namespace Toe.LuaScriptingSystem
{
	public class System : IToeSystem
	{
		#region Constants and Fields

		private readonly ToeSystemConfiguration configuration;

		private readonly ToeScene scene;

		#endregion

		#region Constructors and Destructors

		public System(ToeScene scene, ToeSystemConfiguration configuration)
		{
			this.scene = scene;
			this.configuration = configuration;
		}

		#endregion
	}
}