using OpenTK.Graphics.OpenGL;

namespace Toe.Gx
{
	public static class OpenTKHelper
	{
		public static void Assert()
		{
			ErrorCode ec = GL.GetError();
			if (ec != 0)
			{
				throw new System.ApplicationException(ec.ToString());
			}
		}
	}
}