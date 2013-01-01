using System;
using System.Windows.Forms;

namespace Toe.Resources
{
	public class EditorResourceErrorHandler : ResourceErrorHandler, IResourceErrorHandler
	{
		#region Implementation of IResourceErrorHandler

		public override void CanNotRead(string filePath, Exception exception)
		{
			var res = ResourceErrorDialog.ShowDialogOrDefault(filePath, exception.Message);
			if (res != DialogResult.Ignore)
			{
				base.CanNotRead(filePath,exception);
			}
		}

		#endregion
	}
}