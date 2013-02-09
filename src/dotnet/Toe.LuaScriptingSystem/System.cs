using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Toe.Core;

namespace Toe.LuaScriptingSystem
{
	public class System : IToeSystem
	{
		private readonly ToeScene scene;

		private readonly ToeSystemConfiguration configuration;

		public System(ToeScene scene, ToeSystemConfiguration configuration)
		{
			this.scene = scene;
			this.configuration = configuration;
		}
	}
}
