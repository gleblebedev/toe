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

		private void RenderBox(float size)
		{

			Vector3[] p = new Vector3[]
				{
new Vector3(-size, -size, size),
new Vector3(size, -size, size),
new Vector3(-size, size, size),
new Vector3(size, size, size),
new Vector3(-size, size, -size),
new Vector3(size, size, -size),
new Vector3(-size, -size, -size),
new Vector3(size, -size, -size),
				};
			Vector3[] n = new Vector3[]
				{
new Vector3(-0.57735f, -0.57735f, -0.57735f),
new Vector3(-0.57735f, 0.57735f, -0.57735f),
new Vector3(0.57735f, -0.57735f, -0.57735f),
new Vector3(0.57735f, 0.57735f, -0.57735f),
new Vector3(-0.57735f, -0.57735f, 0.57735f),
new Vector3(-0.57735f, 0.57735f, 0.57735f),
new Vector3(0.57735f, -0.57735f, 0.57735f),
new Vector3(0.57735f, 0.57735f, 0.57735f),
				};
			Vector2[] uv = new Vector2[]
				{
					new Vector2( 0.0f,0.0f ),
					new Vector2( 1.0f,0.0f ),
					new Vector2( 1.0f,1.0f ),
					new Vector2( 0.0f,1.0f ),

				};
			OpenTKHelper.Assert();
			GL.Begin(BeginMode.Quads);

			DrawBoxQuad(p, n, uv, new int[] { 2, 5, 3, 7, 1, 6, 0, 4 });
			DrawBoxQuad(p, n, uv, new int[] { 4, 1, 5, 3, 3, 7, 2, 5 });
			DrawBoxQuad(p, n, uv, new int[] { 6, 0, 7, 2, 5, 3, 4, 1 });
			DrawBoxQuad(p, n, uv, new int[] { 0, 4, 1, 6, 7, 2, 6, 0 });
			DrawBoxQuad(p, n, uv, new int[] { 3, 7, 5, 3, 7, 2, 1, 6 });
			DrawBoxQuad(p, n, uv, new int[] { 4, 1, 2, 5, 0, 4, 6, 0 });

			GL.End();
			OpenTKHelper.Assert();
		}

		private void DrawBoxQuad(Vector3[] p, Vector3[] n, Vector2[] uv, int[] ints)
		{
			for (int i=0; i<4; ++i)
			{
				GL.Normal3(n[ints[i*2+1]]);
				GL.MultiTexCoord2(TextureUnit.Texture1, ref uv[i]);
				GL.TexCoord2(uv[i]);
				GL.Vertex3(p[ints[i * 2 ]]);
			}
		}

		#endregion
	}
}