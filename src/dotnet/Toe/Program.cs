using System;
using System.Windows.Forms;

using Toe.Core;

namespace Toe
{
	internal static class Program
	{
		#region Methods

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			var config = new ToeSceneConfiguration();
			var webViewConfig = new ToeSystemConfiguration(){SystemName = "WebView"};
			webViewConfig.Layers.Add(new ToeLayerConfiguration(){LayerName = "WebView", Popularity = ToeComponentLayerPopularity.VeryRare});
			var luaConfig = new ToeSystemConfiguration() { SystemName = "LuaScript" };
			luaConfig.Layers.Add(new ToeLayerConfiguration() { LayerName = "Script", Popularity = ToeComponentLayerPopularity.Average });
			config.Systems.Add(webViewConfig);
			config.Systems.Add(luaConfig);
			
			Application.Run(new MainWindow(config));
		}

		#endregion
	}
}