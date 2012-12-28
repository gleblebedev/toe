using System;
using System.Windows.Forms;

using Toe.Resources;

namespace Toe.Editor
{
	public class EditorResourceErrorHandler : ResourceErrorHandler, IResourceErrorHandler
	{
		#region Implementation of IResourceErrorHandler

		public override void CanNotRead(string filePath, Exception exception)
		{
			var res = MessageBox.Show("Can't read " + filePath, "Resource error", MessageBoxButtons.AbortRetryIgnore);
			if (res == DialogResult.Abort)
			{
				base.CanNotRead(filePath,exception);
			}
		}

		#endregion
	}
}