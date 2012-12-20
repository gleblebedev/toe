using System;
using System.Windows.Forms;

namespace Toe.mono
{
	class MainClass
	{
		public static void Main (string[] args)
		{
	

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainWindow());
		}
	}
}
