using System;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using Toe.Gx;
using Toe.Utils.Mesh.Marmalade;

namespace Toe.Editors
{
	public abstract class Base3DEditor : UserControl
	{
		#region Constants and Fields

		private readonly EditorCamera camera = new EditorCamera();

		private readonly GLControl gl;

		private ICameraController cameraController;

		private bool loaded;

		#endregion

		#region Constructors and Destructors

		public Base3DEditor()
		{
			this.gl = new GLControl(GraphicsMode.Default, 1, 0, GraphicsContextFlags.Default);
			this.gl.Dock = DockStyle.Fill;
			this.gl.Load += this.GLControlLoad;
			this.gl.Paint += this.GLControlPaint;
			this.gl.Resize += this.GLControlResize;
			this.Controls.Add(this.gl);
			this.gl.MouseMove += this.OnSceneMouseMove;
			this.gl.MouseEnter += this.OnSceneMouseEnter;
			this.gl.MouseLeave += this.OnSceneMouseLeave;
			this.Camera.LookAt(new Vector3(512, 64, 1024), new Vector3(0, 0, 0), new Vector3(0, 0, 1));
			this.CameraController = new Autodesk3DMaxCompatibleController { Camera = this.Camera };
		}

		#endregion

		public void Refresh()
		{
			gl.Invalidate();
		}

		#region Public Properties

		public ICameraController CameraController
		{
			get
			{
				return this.cameraController;
			}
			set
			{
				if (this.cameraController != value)
				{
					if (this.cameraController != null)
					{
						this.cameraController.Detach(this.gl);
					}
					this.cameraController = value;
					if (this.cameraController != null)
					{
						this.cameraController.Attach(this.gl);
					}
				}
			}
		}

		public EditorCamera Camera
		{
			get
			{
				return this.camera;
			}
		}

		#endregion

		#region Methods

		protected abstract void RenderScene();

		private void GLControlLoad(object sender, EventArgs e)
		{
			this.loaded = true;
			this.gl.MakeCurrent();
			this.SetupViewport();
		}

		private void GLControlPaint(object sender, PaintEventArgs e)
		{
			if (!this.loaded) // Play nice
			{
				return;
			}
			this.gl.MakeCurrent();
			GL.Enable(EnableCap.DepthTest);

			this.Camera.SetProjection();
			this.RenderScene();

			GL.Disable(EnableCap.DepthTest);
			GL.Begin(BeginMode.Lines);
			GL.Color3(1.0f, 0, 0);
			GL.Vertex3(0, 0, 0);
			GL.Vertex3(1024, 0, 0);
			GL.Color3(0, 1.0f, 0);
			GL.Vertex3(0, 0, 0);
			GL.Vertex3(0, 1024, 0);
			GL.Color3(0, 0, 1.0f);
			GL.Vertex3(0, 0, 0);
			GL.Vertex3(0, 0, 1024);
			GL.End();
			this.gl.SwapBuffers();
		}


		protected void RenderBox(float size)
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
			for (int i = 0; i < 4; ++i)
			{
				GL.Normal3(n[ints[i * 2 + 1]]);
				GL.MultiTexCoord2(TextureUnit.Texture1, ref uv[i]);
				GL.TexCoord2(uv[i]);
				GL.Vertex3(p[ints[i * 2]]);
			}
		}
		private void GLControlResize(object sender, EventArgs e)
		{
			if (!this.loaded)
			{
				return;
			}
			this.gl.MakeCurrent();
			this.SetupViewport();
		}

		private void OnSceneMouseEnter(object sender, EventArgs e)
		{
			if (this.CameraController != null)
			{
				this.CameraController.MouseEnter();
			}
		}

		private void OnSceneMouseLeave(object sender, EventArgs e)
		{
			if (this.CameraController != null)
			{
				this.CameraController.MouseLeave();
			}
		}

		private void OnSceneMouseMove(object sender, MouseEventArgs e)
		{
			if (this.CameraController != null)
			{
				this.CameraController.MouseMove(e.Button, e.Location);
				this.gl.Refresh();
			}
		}

		private void SetupViewport()
		{
			int w = this.gl.Width;
			int h = this.gl.Height;
			// Use all of the glControl painting area
			GL.Viewport(0, 0, w, h);

			this.Camera.AspectRation = w / (float)h;
		}

		#endregion
	}
}