using OpenTK;
using OpenTK.Graphics.OpenGL;

using Toe.Utils.Mesh.Marmalade;

namespace Toe.Editors.Marmalade
{
	public class MaterialPreview:Base3DEditor
	{
		private readonly MaterialEditor editor;

		public MaterialPreview(MaterialEditor editor)
		{
			this.editor = editor;
			this.Camera.Ortho = false;
			this.Camera.ZNear = 16.0f;
			this.Camera.ZFar = 2048.0f;
		}

		#region Overrides of Base3DEditor

		protected override void RenderScene()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


			if (editor.Material == null)
				return;
			if (editor.Material.Invisible)
				return;

			GL.PushAttrib(AttribMask.AllAttribBits);

			GL.Enable(EnableCap.Lighting);
			GL.Enable(EnableCap.Light0);
			GL.Light(LightName.Light0, LightParameter.Position, new float[]{Camera.Pos.X,Camera.Pos.Y,Camera.Pos.Z});



			editor.Material.ApplyOpenGL();
			RenderBox(250);

			GL.PopAttrib();

		}

		

	

		#endregion
	}
}