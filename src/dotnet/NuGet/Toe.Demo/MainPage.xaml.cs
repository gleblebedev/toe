using System.Windows.Controls;

using Toe.Scene;

namespace Toe.Demo
{
	public partial class MainPage : UserControl
	{
		private SceneGraph scene;

		#region Constructors and Destructors

		public MainPage()
		{
			this.InitializeComponent();
			this.scene = new SceneGraph(new int[]{1024});
		}

		#endregion
	}
}