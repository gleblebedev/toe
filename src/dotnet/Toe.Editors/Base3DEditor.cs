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

		private GLControl glControl;

		private ICameraController cameraController;
		private ToolStrip toolStrip1;
		private ToolStripDropDownButton toolStripDropDownButton1;
		private ToolStripMenuItem zUpToolStripMenuItem;
		private ToolStripMenuItem yUpToolStripMenuItem;
		//private GLControl glControl;

		private bool loaded;

		#endregion

		#region Constructors and Destructors

		public Base3DEditor()
		{
			this.InitializeComponent();
			//this.glControl = new GLControl(GraphicsMode.Default, 1, 0, GraphicsContextFlags.Default);
			//this.glControl.Dock = DockStyle.Fill;
			this.glControl.Load += this.GLControlLoad;
			this.glControl.Paint += this.GLControlPaint;
			this.glControl.Resize += this.GLControlResize;
			this.Controls.Add(this.glControl);
			this.glControl.MouseMove += this.OnSceneMouseMove;
			this.glControl.MouseEnter += this.OnSceneMouseEnter;
			this.glControl.MouseLeave += this.OnSceneMouseLeave;
			this.Camera.LookAt(new Vector3(512, 64, 1024), new Vector3(0, 0, 0), new Vector3(0, 0, 1));
			this.CameraController = new Autodesk3DMaxCompatibleController { Camera = this.Camera };
		}

		#endregion

		public void Refresh()
		{
			this.glControl.Invalidate();
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
						this.cameraController.Detach(this.glControl);
					}
					this.cameraController = value;
					if (this.cameraController != null)
					{
						this.cameraController.Attach(this.glControl);
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
			this.glControl.MakeCurrent();
			this.SetupViewport();
		}

		private void GLControlPaint(object sender, PaintEventArgs e)
		{
			if (!this.loaded) // Play nice
			{
				return;
			}
			this.glControl.MakeCurrent();
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
			this.glControl.SwapBuffers();
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
			this.glControl.MakeCurrent();
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
				this.glControl.Refresh();
			}
		}

		private void SetupViewport()
		{
			int w = this.glControl.Width;
			int h = this.glControl.Height;
			// Use all of the glControl painting area
			GL.Viewport(0, 0, w, h);

			this.Camera.AspectRation = w / (float)h;
		}

		#endregion

		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Base3DEditor));
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
			this.zUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.yUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.glControl = new OpenTK.GLControl();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(150, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripDropDownButton1
			// 
			this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zUpToolStripMenuItem,
            this.yUpToolStripMenuItem});
			this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
			this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
			this.toolStripDropDownButton1.Size = new System.Drawing.Size(29, 22);
			this.toolStripDropDownButton1.Text = "toolStripDropDownButton1";
			// 
			// zUpToolStripMenuItem
			// 
			this.zUpToolStripMenuItem.Name = "zUpToolStripMenuItem";
			this.zUpToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.zUpToolStripMenuItem.Text = "Z-Up";
			this.zUpToolStripMenuItem.Click += new System.EventHandler(this.SelectZUp);
			// 
			// yUpToolStripMenuItem
			// 
			this.yUpToolStripMenuItem.Name = "yUpToolStripMenuItem";
			this.yUpToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.yUpToolStripMenuItem.Text = "Y-Up";
			this.yUpToolStripMenuItem.Click += new System.EventHandler(this.SelectYUp);
			// 
			// glControl
			// 
			this.glControl.BackColor = System.Drawing.Color.Black;
			this.glControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.glControl.Location = new System.Drawing.Point(0, 25);
			this.glControl.Name = "glControl";
			this.glControl.Size = new System.Drawing.Size(150, 125);
			this.glControl.TabIndex = 1;
			this.glControl.VSync = false;
			// 
			// Base3DEditor
			// 
			this.Controls.Add(this.glControl);
			this.Controls.Add(this.toolStrip1);
			this.Name = "Base3DEditor";
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private void SelectZUp(object sender, EventArgs e)
		{

		}

		private void SelectYUp(object sender, EventArgs e)
		{

		}
	}
}