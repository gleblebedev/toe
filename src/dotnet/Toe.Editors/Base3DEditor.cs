using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using Autofac;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Panels;
using Toe.Editors.Properties;
using Toe.Gx;
using Toe.Marmalade.IwGx;
using Toe.Resources;
using Toe.Utils.Mesh;
using Toe.Utils.Mesh.Ase;

using Resources=Toe.Editors.Properties.Resources;

namespace Toe.Editors
{
	public class Base3DEditor : UserControl
	{
		#region Constants and Fields

		protected readonly IEditorOptions<Base3DEditorOptions> options;

		private readonly Base3DEditorContent content;

		private readonly EditorCamera camera;

		private ICameraController cameraController;

		private ToolStripDropDownButton coordinateSystemButton;

		private Button errButton;

		private Label errMessage;

		private StackPanel errPanel;

		private GLControl glControl;

		private ToolStripButton lightingButton;

		private bool loaded;

		private ToolStrip toolStrip1;

		private ToolStripMenuItem yUpToolStripMenuItem;

		private ToolStripMenuItem zUpToolStripMenuItem;

		private ToeGraphicsContext graphicsContext;

		#endregion

		//private GLControl glControl;

		#region Constructors and Destructors

		public Base3DEditor(IComponentContext context, IEditorOptions<Base3DEditorOptions> options,Base3DEditorContent content)
		{
			this.options = options;
			this.content = content;
			graphicsContext = context.Resolve<ToeGraphicsContext>();
			this.camera = new EditorCamera(context.Resolve<IEditorOptions<EditorCameraOptions>>());

			this.InitializeComponent();
			this.Dock = DockStyle.Fill;
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
			this.Camera.LookAt(new Vector3(512, 64, 1024), new Vector3(0, 0, 0));
			this.CameraController = new TargetCameraController { Camera = this.Camera };
			this.yUpToolStripMenuItem.Click += this.SelectYUp;
			this.zUpToolStripMenuItem.Click += this.SelectZUp;
			this.UpdateCoordinateSystemIcon();
			this.UpdateLighingIcon();
		}

		#endregion

		#region Public Events

		public event EventHandler RenderScene;

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
						this.cameraController.Camera = null;
					}
					this.cameraController = value;
					if (this.cameraController != null)
					{
						this.cameraController.Attach(this.glControl);
						this.cameraController.Camera = this.camera;
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

		private LightArgs light = LightArgs.Default;
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
			this.SetupViewport(graphicsContext);
		}

		private void GLControlPaint(object sender, PaintEventArgs e)
		{
			if (!this.loaded) // Play nice
			{
				return;
			}
			try
			{
				this.glControl.MakeCurrent();

				if (GraphicsContext.CurrentContext == null)
				{
					return;
				}

				this.SetupViewport(graphicsContext);

				GL.Enable(EnableCap.DepthTest);
				this.Camera.SetProjection(graphicsContext);

				if (this.options.Options.Lighting)
				{
					light.Enabled = true;
					light.Position = this.Camera.Pos;
					graphicsContext.SetLight0(ref light);
					graphicsContext.EnableLighting();
				}
				else
				{
					graphicsContext.DisableLighting();
				}

				if (this.RenderScene != null)
				{
					this.RenderScene(this, new EventArgs());
				}

				GL.UseProgram(0);
				graphicsContext.SetMaterial(null);

				graphicsContext.DisableLighting();
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
				OpenTKHelper.Assert();

				content.RenderXyzCube(glControl,Camera);

				GL.Flush();
				OpenTKHelper.Assert();
				this.glControl.SwapBuffers();
			}
			catch (Exception ex)
			{
				this.glControl.Visible = false;
				this.errPanel.Visible = true;
				this.errMessage.Text = ex.ToString();
			}
		}

	

		private void GLControlResize(object sender, EventArgs e)
		{
			if (!this.loaded)
			{
				return;
			}
			this.glControl.MakeCurrent();
			this.SetupViewport(graphicsContext);
		}

		private void InitializeComponent()
		{
			this.toolStrip1 = new ToolStrip();
			this.coordinateSystemButton = new ToolStripDropDownButton();
			this.zUpToolStripMenuItem = new ToolStripMenuItem();
			this.yUpToolStripMenuItem = new ToolStripMenuItem();
			this.lightingButton = new ToolStripButton();
			this.glControl = new GLControl();
			this.errPanel = new StackPanel();
			this.errButton = new Button();
			this.errMessage = new Label();
			this.toolStrip1.SuspendLayout();
			this.errPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.coordinateSystemButton, this.lightingButton });
			this.toolStrip1.Location = new Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new Size(150, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// coordinateSystemButton
			// 
			this.coordinateSystemButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.coordinateSystemButton.DropDownItems.AddRange(
				new ToolStripItem[] { this.zUpToolStripMenuItem, this.yUpToolStripMenuItem });
			this.coordinateSystemButton.Image = Toe.Editors.Properties.Resources.zup;
			this.coordinateSystemButton.ImageTransparentColor = Color.Magenta;
			this.coordinateSystemButton.Name = "coordinateSystemButton";
			this.coordinateSystemButton.Size = new Size(29, 22);
			this.coordinateSystemButton.Text = "toolStripDropDownButton1";
			// 
			// zUpToolStripMenuItem
			// 
			this.zUpToolStripMenuItem.Image = Toe.Editors.Properties.Resources.zup;
			this.zUpToolStripMenuItem.Name = "zUpToolStripMenuItem";
			this.zUpToolStripMenuItem.Size = new Size(152, 22);
			this.zUpToolStripMenuItem.Text = "Z-Up";
			// 
			// yUpToolStripMenuItem
			// 
			this.yUpToolStripMenuItem.Image = Toe.Editors.Properties.Resources.yup;
			this.yUpToolStripMenuItem.Name = "yUpToolStripMenuItem";
			this.yUpToolStripMenuItem.Size = new Size(152, 22);
			this.yUpToolStripMenuItem.Text = "Y-Up";
			// 
			// lightingButton
			// 
			this.lightingButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.lightingButton.ImageTransparentColor = Color.Magenta;
			this.lightingButton.Name = "lightingButton";
			this.lightingButton.Size = new Size(23, 22);
			this.lightingButton.Text = "lighingButton";
			this.lightingButton.Click += this.ToggleLightingClick;
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
			// errPanel
			// 
			this.errPanel.AutoScroll = true;
			this.errPanel.Controls.Add(this.errButton);
			this.errPanel.Controls.Add(this.errMessage);
			this.errPanel.Dock = DockStyle.Fill;
			this.errPanel.Location = new Point(0, 25);
			this.errPanel.Name = "errPanel";
			this.errPanel.Size = new Size(150, 125);
			this.errPanel.TabIndex = 2;
			this.errPanel.Visible = false;
			// 
			// errButton
			// 
			this.errButton.Location = new Point(3, 3);
			this.errButton.Name = "errButton";
			this.errButton.Size = new Size(150, 23);
			this.errButton.TabIndex = 1;
			this.errButton.Text = "Retry";
			this.errButton.UseVisualStyleBackColor = true;
			this.errButton.Click += this.errButton_Click;
			// 
			// errMessage
			// 
			this.errMessage.AutoSize = true;
			this.errMessage.Location = new Point(3, 29);
			this.errMessage.Name = "errMessage";
			this.errMessage.Size = new Size(150, 13);
			this.errMessage.TabIndex = 0;
			this.errMessage.Text = "Render error";
			// 
			// Base3DEditor
			// 
			this.Controls.Add(this.errPanel);
			this.Controls.Add(this.glControl);
			this.Controls.Add(this.toolStrip1);
			this.Name = "Base3DEditor";
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.errPanel.ResumeLayout(false);
			this.errPanel.PerformLayout();
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
				this.RefreshScene();
			}
		}

		private void OnSceneMouseWheel(object sender, MouseEventArgs e)
		{
			if (this.CameraController != null)
			{
				this.CameraController.MouseWheel(e.Delta, e.Location);
				this.RefreshScene();
			}
		}

		private void SelectYUp(object sender, EventArgs e)
		{
			if (this.Camera.CoordinateSystem != CoordinateSystem.YUp)
			{
				this.Camera.CoordinateSystem = CoordinateSystem.YUp;
				this.UpdateCoordinateSystemIcon();
			}
		}

		private void SelectZUp(object sender, EventArgs e)
		{
			if (this.Camera.CoordinateSystem != CoordinateSystem.ZUp)
			{
				this.Camera.CoordinateSystem = CoordinateSystem.ZUp;
				this.UpdateCoordinateSystemIcon();
			}
		}

		private void SetupViewport(ToeGraphicsContext context)
		{
			if (!this.loaded)
			{
				return;
			}
			if (GraphicsContext.CurrentContext == null)
			{
				return;
			}
			int w = Math.Max(1, this.glControl.Width);
			int h = Math.Max(1, this.glControl.Height);
			// Use all of the glControl painting area
			graphicsContext.SetViewport(0, 0, w, h);

			this.Camera.AspectRation = w / (float)h;
		}

		private void ToggleLightingClick(object sender, EventArgs e)
		{
			this.options.Options.Lighting = !this.options.Options.Lighting;
			this.UpdateLighingIcon();
			this.RefreshScene();
		}

		private void UpdateCoordinateSystemIcon()
		{
			switch (this.Camera.CoordinateSystem)
			{
				case CoordinateSystem.ZUp:
					this.coordinateSystemButton.Image = Toe.Editors.Properties.Resources.zup;
					break;
				case CoordinateSystem.YUp:
					this.coordinateSystemButton.Image = Toe.Editors.Properties.Resources.yup;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void UpdateLighingIcon()
		{
			if (this.options.Options.Lighting)
			{
				this.lightingButton.Image = Toe.Editors.Properties.Resources.light_on;
			}
			else
			{
				this.lightingButton.Image = Toe.Editors.Properties.Resources.light_off;
			}
		}

		private void errButton_Click(object sender, EventArgs e)
		{
			this.glControl.Visible = true;
			this.errPanel.Visible = false;
		}

		#endregion
	}
}