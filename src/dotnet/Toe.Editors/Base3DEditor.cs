using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Toe.Gx;

namespace Toe.Editors
{
	public abstract class Base3DEditor : UserControl
	{
		#region Constants and Fields

		private readonly EditorCamera camera = new EditorCamera();

		private ICameraController cameraController;

		private GLControl glControl;

		private bool loaded;

		private ToolStrip toolStrip1;

		private ToolStripDropDownButton toolStripDropDownButton1;

		private ToolStripMenuItem yUpToolStripMenuItem;

		private ToolStripMenuItem zUpToolStripMenuItem;

		#endregion

		//private GLControl glControl;

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
			this.glControl.MouseWheel += this.OnSceneMouseWheel;
			this.Camera.LookAt(new Vector3(512, 64, 1024), new Vector3(0, 0, 0), this.Camera.WorldUp);
			this.CameraController = new TargetCameraController { Camera = this.Camera };
		}

		#endregion

		#region Public Properties

		public EditorCamera Camera
		{
			get
			{
				return this.camera;
			}
		}

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

		#endregion

		#region Public Methods and Operators

		public void RefreshScene()
		{
			this.glControl.Invalidate();
		}

		#endregion

		#region Methods

		protected void RenderBox(float size)
		{
			Vector3[] p = new[]
				{
					new Vector3(-size, -size, size), new Vector3(size, -size, size), new Vector3(-size, size, size),
					new Vector3(size, size, size), new Vector3(-size, size, -size), new Vector3(size, size, -size),
					new Vector3(-size, -size, -size), new Vector3(size, -size, -size),
				};
			Vector3[] n = new[]
				{
					new Vector3(-0.57735f, -0.57735f, -0.57735f), new Vector3(-0.57735f, 0.57735f, -0.57735f),
					new Vector3(0.57735f, -0.57735f, -0.57735f), new Vector3(0.57735f, 0.57735f, -0.57735f),
					new Vector3(-0.57735f, -0.57735f, 0.57735f), new Vector3(-0.57735f, 0.57735f, 0.57735f),
					new Vector3(0.57735f, -0.57735f, 0.57735f), new Vector3(0.57735f, 0.57735f, 0.57735f),
				};
			Vector2[] uv = new[]
				{ new Vector2(0.0f, 0.0f), new Vector2(1.0f, 0.0f), new Vector2(1.0f, 1.0f), new Vector2(0.0f, 1.0f), };
			OpenTKHelper.Assert();
			GL.Begin(BeginMode.Quads);

			this.DrawBoxQuad(p, n, uv, new[] { 2, 5, 3, 7, 1, 6, 0, 4 });
			this.DrawBoxQuad(p, n, uv, new[] { 4, 1, 5, 3, 3, 7, 2, 5 });
			this.DrawBoxQuad(p, n, uv, new[] { 6, 0, 7, 2, 5, 3, 4, 1 });
			this.DrawBoxQuad(p, n, uv, new[] { 0, 4, 1, 6, 7, 2, 6, 0 });
			this.DrawBoxQuad(p, n, uv, new[] { 3, 7, 5, 3, 7, 2, 1, 6 });
			this.DrawBoxQuad(p, n, uv, new[] { 4, 1, 2, 5, 0, 4, 6, 0 });

			GL.End();
			OpenTKHelper.Assert();
		}

		protected abstract void RenderScene();

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
			GL.Flush();
			this.glControl.SwapBuffers();
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

		private void InitializeComponent()
		{
			ComponentResourceManager resources = new ComponentResourceManager(typeof(Base3DEditor));
			this.toolStrip1 = new ToolStrip();
			this.toolStripDropDownButton1 = new ToolStripDropDownButton();
			this.zUpToolStripMenuItem = new ToolStripMenuItem();
			this.yUpToolStripMenuItem = new ToolStripMenuItem();
			this.glControl = new GLControl();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.toolStripDropDownButton1 });
			this.toolStrip1.Location = new Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new Size(150, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripDropDownButton1
			// 
			this.toolStripDropDownButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.toolStripDropDownButton1.DropDownItems.AddRange(
				new ToolStripItem[] { this.zUpToolStripMenuItem, this.yUpToolStripMenuItem });
			this.toolStripDropDownButton1.Image = ((Image)(resources.GetObject("toolStripDropDownButton1.Image")));
			this.toolStripDropDownButton1.ImageTransparentColor = Color.Magenta;
			this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
			this.toolStripDropDownButton1.Size = new Size(29, 22);
			this.toolStripDropDownButton1.Text = "toolStripDropDownButton1";
			// 
			// zUpToolStripMenuItem
			// 
			this.zUpToolStripMenuItem.Name = "zUpToolStripMenuItem";
			this.zUpToolStripMenuItem.Size = new Size(152, 22);
			this.zUpToolStripMenuItem.Text = "Z-Up";
			this.zUpToolStripMenuItem.Click += this.SelectZUp;
			// 
			// yUpToolStripMenuItem
			// 
			this.yUpToolStripMenuItem.Name = "yUpToolStripMenuItem";
			this.yUpToolStripMenuItem.Size = new Size(152, 22);
			this.yUpToolStripMenuItem.Text = "Y-Up";
			this.yUpToolStripMenuItem.Click += this.SelectYUp;
			// 
			// glControl
			// 
			this.glControl.BackColor = Color.Black;
			this.glControl.Dock = DockStyle.Fill;
			this.glControl.Location = new Point(0, 25);
			this.glControl.Name = "glControl";
			this.glControl.Size = new Size(150, 125);
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

		private void OnSceneMouseWheel(object sender, MouseEventArgs e)
		{
			if (this.CameraController != null)
			{
				this.CameraController.MouseWheel(e.Delta, e.Location);
				this.glControl.Refresh();
			}
		}

		private void SelectYUp(object sender, EventArgs e)
		{
			if (this.Camera.CoordinateSystem != CoordinateSystem.YUp)
			{
				this.Camera.CoordinateSystem = CoordinateSystem.YUp;
				this.Camera.LookAt(new Vector3(512, 64, 1024), new Vector3(0, 0, 0), this.Camera.WorldUp);
			}
		}

		private void SelectZUp(object sender, EventArgs e)
		{
			if (this.Camera.CoordinateSystem != CoordinateSystem.ZUp)
			{
				this.Camera.CoordinateSystem = CoordinateSystem.ZUp;
				this.Camera.LookAt(new Vector3(512, 64, 1024), new Vector3(0, 0, 0), this.Camera.WorldUp);
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
	}
}