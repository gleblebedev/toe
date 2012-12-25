using OpenTK.Graphics.OpenGL;

namespace Toe.Editors.Marmalade
{
	public class MaterialPreview:Base3DEditor
	{
		#region Overrides of Base3DEditor

		protected override void RenderScene()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
		}

		#endregion
	}
}