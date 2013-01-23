using System;
using System.Diagnostics;

using OpenTK.Graphics.OpenGL;

namespace Toe.Gx
{
	public static class OpenTKHelper
	{
		#region Public Methods and Operators

		public static void Assert()
		{
			ErrorCode ec = GL.GetError();
			if (ec != 0)
			{
				if (Debugger.IsAttached)
					Debugger.Break();
				throw new ApplicationException(ec.ToString());
			}
		}

		#endregion
	}
}