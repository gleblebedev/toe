using System;

using Autofac;

using OpenTK.Graphics.OpenGL;

using Toe.Editors.Interfaces;
using Toe.Gx;
using Toe.Utils.Mesh;

namespace Toe.Editors.Marmalade
{
	public class MaterialPreview : Base3DEditor
	{
		#region Constants and Fields

		private readonly MaterialEditor editor;

		private readonly ToeGraphicsContext graphicsContext;

		private IMesh box;

		#endregion

		#region Constructors and Destructors

		public MaterialPreview(
			MaterialEditor editor,
			ToeGraphicsContext graphicsContext,
			IComponentContext context,
			IEditorOptions<Base3DEditorOptions> options, Base3DEditorContent content)
			: base(context, options, content)
		{
			this.editor = editor;
			this.graphicsContext = graphicsContext;
			this.Camera.Ortho = false;
			this.Camera.ZNear = 16.0f;
			this.Camera.ZFar = 2048.0f;
			base.RenderScene += this.RenderScene;
			box = BoxBuilder.BuildSoftEdgedBox(250);
		}

		#endregion

		#region Methods

		protected new void RenderScene(object sender, EventArgs args)
		{
			GL.ClearColor(0.2f, 0.2f, 0.4f, 1f);

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

			if (this.options.Options.Lighting)
			{
				GL.Enable(EnableCap.Lighting);
				GL.Enable(EnableCap.Light0);
				GL.Light(
					LightName.Light0, LightParameter.Position, new[] { this.Camera.Pos.X, this.Camera.Pos.Y, this.Camera.Pos.Z, 1.0f });
			}
			else
			{
				GL.Disable(EnableCap.Lighting);
			}
			graphicsContext.SetMaterial(this.editor.Material);
			graphicsContext.Render(box);
			graphicsContext.SetMaterial(null);

			GL.PopAttrib();
		}

		#endregion
	}
}