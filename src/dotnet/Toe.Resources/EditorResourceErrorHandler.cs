using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Toe.Resources
{
	public class EditorResourceErrorHandler : ResourceErrorHandler, IResourceErrorHandler
	{
		#region Public Methods and Operators

		public override void CanNotRead(string filePath, Exception exception)
		{
			Trace.WriteLine(exception);
			if (Debugger.IsAttached)
			{
				Debugger.Break();
			}
			var res = ResourceErrorDialog.ShowDialogOrDefault(filePath, exception.Message);
			if (res != DialogResult.Ignore)
			{
				base.CanNotRead(filePath, exception);
			}
		}

		#endregion
	}
}