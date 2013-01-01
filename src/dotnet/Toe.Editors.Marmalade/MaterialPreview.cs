using OpenTK.Graphics.OpenGL;

namespace Toe.Editors.Marmalade
{
	public class MaterialPreview : Base3DEditor
	{
		#region Constants and Fields

		private readonly MaterialEditor editor;

		#endregion

		#region Constructors and Destructors

		public MaterialPreview(MaterialEditor editor)
		{
			this.editor = editor;
			this.Camera.Ortho = false;
			this.Camera.ZNear = 16.0f;
			this.Camera.ZFar = 2048.0f;
		}

		#endregion

		#region Methods

		protected override void RenderScene()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			if (this.editor.Material == null)
			{
				return;
			}
			if (this.editor.Material.Invisible)
			{
				return;
			}

			GL.PushAttrib(AttribMask.AllAttribBits);

			GL.Enable(EnableCap.Lighting);
			GL.Enable(EnableCap.Light0);
			GL.Light(
				LightName.Light0, LightParameter.Position, new[] { this.Camera.Pos.X, this.Camera.Pos.Y, this.Camera.Pos.Z });

			this.editor.Material.ApplyOpenGL();
			this.RenderBox(250);

			GL.PopAttrib();
		}

		#endregion
	}
}