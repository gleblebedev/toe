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

		private readonly IMesh box;

		private readonly MaterialEditor editor;

		private readonly ToeGraphicsContext graphicsContext;

		#endregion

		#region Constructors and Destructors

		public MaterialPreview(
			MaterialEditor editor,
			ToeGraphicsContext graphicsContext,
			IComponentContext context,
			IEditorOptions<Base3DEditorOptions> options,
			Base3DEditorContent content,
			IStreamConverterFactory streamConverterFactory)
			: base(context, options, content)
		{
			this.editor = editor;
			this.graphicsContext = graphicsContext;
			this.Camera.Ortho = false;
			this.Camera.ZNear = 16.0f;
			this.Camera.ZFar = 2048.0f;
			base.RenderScene += this.RenderMaterialScene;
			this.box = BoxBuilder.BuildSoftEdgedBox(250, streamConverterFactory);
		}

		#endregion

		//private LightArgs light = LightArgs.Default;

		#region Methods

		protected void RenderMaterialScene(object sender, EventArgs args)
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

			//GL.PushAttrib(AttribMask.AllAttribBits);

			//if (this.options.Options.Lighting)
			//{
			//    light.Position = this.Camera.Pos;
			//    graphicsContext.SetLight0(ref light);
			//}
			//else
			//{
			//    GL.Disable(EnableCap.Lighting);
			//}
			this.graphicsContext.SetMaterial(this.editor.Material);
			this.RenderMesh(this.box);
			this.graphicsContext.SetMaterial(null);

			//GL.PopAttrib();
		}

		#endregion
	}
}