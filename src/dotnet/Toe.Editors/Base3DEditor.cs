using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

using Autofac;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Panels;
using Toe.Gx;
using Toe.Utils.Mesh;

using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace Toe.Editors
{
	public class Base3DEditor : UserControl
	{
		#region Constants and Fields

		protected readonly IEditorOptions<Base3DEditorOptions> options;

		private readonly EditorCamera camera;

		private readonly Base3DEditorContent content;

		private readonly ToeGraphicsContext graphicsContext;

		private ToolStripButton btnScreenShot;

		private ICameraController cameraController;

		private ToolStripDropDownButton coordinateSystemButton;

		private Button errButton;

		private Label errMessage;

		private StackPanel errPanel;

		private GLControl glControl;

		private ToolStripMenuItem hideNormalsMenuItem;

		private ToolStripMenuItem hideWireframeMenuItem;

		private Timer keysAnimationTimer;

		private LightArgs light = LightArgs.Default;

		private ToolStripButton lightingButton;

		private bool loaded;

		private ToolStripDropDownButton renderNormalsButton;

		private ToolStripMenuItem renderNormalsMenuItem;

		private ToolStripDropDownButton renderWireButton;

		private ToolStripMenuItem renderWireframeMenuItem;

		private ToolStrip toolStrip1;

		private ToolStripMenuItem yUpToolStripMenuItem;

		private ToolStripMenuItem zUpToolStripMenuItem;

		#endregion

		//private GLControl glControl;

		#region Constructors and Destructors

		public Base3DEditor(
			IComponentContext context, IEditorOptions<Base3DEditorOptions> options, Base3DEditorContent content)
		{
			this.options = options;
			this.content = content;
			this.graphicsContext = context.Resolve<ToeGraphicsContext>();
			this.camera = new EditorCamera(context.Resolve<IEditorOptions<EditorCameraOptions>>());
			this.camera.PropertyChanged += this.OnCameraPropertyChanged;
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
			this.glControl.KeyDown += this.OnControlKeyDown;
			this.glControl.KeyUp += this.OnControlKeyUp;
			this.glControl.GotFocus += this.OnSceneGotFocus;
			this.glControl.LostFocus += this.OnSceneLostFocus;
			this.Camera.LookAt(new Vector3(512, 64, 1024), new Vector3(0, 0, 0));
			this.CameraController = new TargetCameraController { Camera = this.Camera };
			this.yUpToolStripMenuItem.Click += this.SelectYUp;
			this.zUpToolStripMenuItem.Click += this.SelectZUp;
			this.UpdateCoordinateSystemIcon();
			this.UpdateLighingIcon();
			this.UpdateWireframeIcon();
			this.UpdateNormalIcon();
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

		public void RenderMesh(IMesh mesh)
		{
			this.graphicsContext.Render(mesh);
			if (this.options.Options.Wireframe)
			{
				this.RenderMeshWireframe(mesh);
			}
			if (this.options.Options.Normals)
			{
				this.RenderMeshNormals(mesh);
			}
		}

		#endregion

		#region Methods

		protected void OnControlKeyDown(object sender, KeyEventArgs e)
		{
			if (this.cameraController != null)
			{
				this.cameraController.KeyDown(e);
			}
		}

		protected void OnControlKeyUp(object sender, KeyEventArgs e)
		{
			if (this.cameraController != null)
			{
				this.cameraController.KeyUp(e);
			}
		}

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

		protected void SelectNormalsOff(object sender, EventArgs e)
		{
			if (this.options.Options.Normals != false)
			{
				this.options.Options.Normals = false;
				UpdateNormalIcon();
			}
		}
		void UpdateWireframeIcon()
		{
			this.renderWireButton.Image = this.options.Options.Wireframe != true ? this.hideWireframeMenuItem.Image : this.renderWireframeMenuItem.Image;
		}

		void UpdateNormalIcon()
		{
			this.renderNormalsButton.Image = this.options.Options.Normals != true ? this.hideNormalsMenuItem.Image : this.renderNormalsMenuItem.Image;
		}

		protected void SelectNormalsOn(object sender, EventArgs e)
		{
			if (this.options.Options.Normals != true)
			{
				this.options.Options.Normals = true;
				UpdateNormalIcon();
			}
		}

		protected void SelectWireframeOff(object sender, EventArgs e)
		{
			if (this.options.Options.Wireframe)
			{
				this.options.Options.Wireframe = false;
				this.renderWireButton.Image = this.hideWireframeMenuItem.Image;
			}
		}

		protected void SelectWireframeOn(object sender, EventArgs e)
		{
			if (this.options.Options.Wireframe != true)
			{
				this.options.Options.Wireframe = true;
				this.renderWireButton.Image = this.renderWireframeMenuItem.Image;
				
			}
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

		private void GLControlLoad(object sender, EventArgs e)
		{
			this.loaded = true;
			this.glControl.MakeCurrent();
			this.SetupViewport(this.graphicsContext);
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

				this.SetupViewport(this.graphicsContext);

				GL.Enable(EnableCap.DepthTest);
				this.Camera.SetProjection(this.graphicsContext);

				if (this.options.Options.Lighting)
				{
					this.light.Enabled = true;
					this.light.Position = this.Camera.Pos;
					this.graphicsContext.SetLight0(ref this.light);
					this.graphicsContext.EnableLighting();
				}
				else
				{
					this.graphicsContext.DisableLighting();
				}

				if (this.RenderScene != null)
				{
					this.RenderScene(this, new EventArgs());
				}

				GL.UseProgram(0);
				this.graphicsContext.SetMaterial(null);
				this.graphicsContext.DisableLighting();
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

				this.content.RenderXyzCube(this.glControl, this.Camera);

				this.graphicsContext.Flush();

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
			this.SetupViewport(this.graphicsContext);
		}

		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Base3DEditor));
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.lightingButton = new System.Windows.Forms.ToolStripButton();
			this.glControl = new OpenTK.GLControl();
			this.errPanel = new Toe.Editors.Interfaces.Panels.StackPanel();
			this.errButton = new System.Windows.Forms.Button();
			this.errMessage = new System.Windows.Forms.Label();
			this.coordinateSystemButton = new System.Windows.Forms.ToolStripDropDownButton();
			this.zUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.yUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.renderWireButton = new System.Windows.Forms.ToolStripDropDownButton();
			this.renderWireframeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.hideWireframeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.renderNormalsButton = new System.Windows.Forms.ToolStripDropDownButton();
			this.renderNormalsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.hideNormalsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.btnScreenShot = new System.Windows.Forms.ToolStripButton();
			this.toolStrip1.SuspendLayout();
			this.errPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.coordinateSystemButton,
            this.lightingButton,
            this.renderWireButton,
            this.renderNormalsButton,
            this.btnScreenShot});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(550, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// lightingButton
			// 
			this.lightingButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.lightingButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.lightingButton.Name = "lightingButton";
			this.lightingButton.Size = new System.Drawing.Size(23, 22);
			this.lightingButton.Text = "Toggle Lights";
			this.lightingButton.Click += new System.EventHandler(this.ToggleLightingClick);
			// 
			// glControl
			// 
			this.glControl.BackColor = System.Drawing.Color.Black;
			this.glControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.glControl.Location = new System.Drawing.Point(0, 25);
			this.glControl.Name = "glControl";
			this.glControl.Size = new System.Drawing.Size(550, 425);
			this.glControl.TabIndex = 1;
			this.glControl.VSync = false;
			// 
			// errPanel
			// 
			this.errPanel.AutoScroll = true;
			this.errPanel.Controls.Add(this.errButton);
			this.errPanel.Controls.Add(this.errMessage);
			this.errPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.errPanel.Location = new System.Drawing.Point(0, 25);
			this.errPanel.Name = "errPanel";
			this.errPanel.Size = new System.Drawing.Size(550, 425);
			this.errPanel.TabIndex = 2;
			this.errPanel.Visible = false;
			// 
			// errButton
			// 
			this.errButton.Location = new System.Drawing.Point(3, 3);
			this.errButton.Name = "errButton";
			this.errButton.Size = new System.Drawing.Size(550, 23);
			this.errButton.TabIndex = 1;
			this.errButton.Text = "Retry";
			this.errButton.UseVisualStyleBackColor = true;
			// 
			// errMessage
			// 
			this.errMessage.AutoSize = true;
			this.errMessage.Location = new System.Drawing.Point(3, 29);
			this.errMessage.Name = "errMessage";
			this.errMessage.Size = new System.Drawing.Size(550, 13);
			this.errMessage.TabIndex = 0;
			this.errMessage.Text = "Render error";
			// 
			// coordinateSystemButton
			// 
			this.coordinateSystemButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.coordinateSystemButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zUpToolStripMenuItem,
            this.yUpToolStripMenuItem});
			this.coordinateSystemButton.Image = global::Toe.Editors.Properties.Resources.zup;
			this.coordinateSystemButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.coordinateSystemButton.Name = "coordinateSystemButton";
			this.coordinateSystemButton.Size = new System.Drawing.Size(29, 22);
			this.coordinateSystemButton.Text = "Up";
			// 
			// zUpToolStripMenuItem
			// 
			this.zUpToolStripMenuItem.Image = global::Toe.Editors.Properties.Resources.zup;
			this.zUpToolStripMenuItem.Name = "zUpToolStripMenuItem";
			this.zUpToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
			this.zUpToolStripMenuItem.Text = "Z-Up";
			// 
			// yUpToolStripMenuItem
			// 
			this.yUpToolStripMenuItem.Image = global::Toe.Editors.Properties.Resources.yup;
			this.yUpToolStripMenuItem.Name = "yUpToolStripMenuItem";
			this.yUpToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
			this.yUpToolStripMenuItem.Text = "Y-Up";
			// 
			// renderWireButton
			// 
			this.renderWireButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.renderWireButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renderWireframeMenuItem,
            this.hideWireframeMenuItem});
			this.renderWireButton.Image = global::Toe.Editors.Properties.Resources.wireframe_on;
			this.renderWireButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.renderWireButton.Name = "renderWireButton";
			this.renderWireButton.Size = new System.Drawing.Size(29, 22);
			this.renderWireButton.Text = "Toggle Wireframe";
			// 
			// renderWireframeMenuItem
			// 
			this.renderWireframeMenuItem.Image = global::Toe.Editors.Properties.Resources.wireframe_on;
			this.renderWireframeMenuItem.Name = "renderWireframeMenuItem";
			this.renderWireframeMenuItem.Size = new System.Drawing.Size(169, 22);
			this.renderWireframeMenuItem.Text = "Render Wireframe";
			this.renderWireframeMenuItem.Click += new System.EventHandler(this.SelectWireframeOn);
			// 
			// hideWireframeMenuItem
			// 
			this.hideWireframeMenuItem.Image = global::Toe.Editors.Properties.Resources.wireframe_off;
			this.hideWireframeMenuItem.Name = "hideWireframeMenuItem";
			this.hideWireframeMenuItem.Size = new System.Drawing.Size(169, 22);
			this.hideWireframeMenuItem.Text = "Hide Wireframe";
			this.hideWireframeMenuItem.Click += new System.EventHandler(this.SelectWireframeOff);
			// 
			// renderNormalsButton
			// 
			this.renderNormalsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.renderNormalsButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renderNormalsMenuItem,
            this.hideNormalsMenuItem});
			this.renderNormalsButton.Image = global::Toe.Editors.Properties.Resources.normal_on;
			this.renderNormalsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.renderNormalsButton.Name = "renderNormalsButton";
			this.renderNormalsButton.Size = new System.Drawing.Size(29, 22);
			this.renderNormalsButton.Text = "Toggle Normals";
			this.renderNormalsButton.Click += new System.EventHandler(this.SelectNormalsOn);
			// 
			// renderNormalsMenuItem
			// 
			this.renderNormalsMenuItem.Image = global::Toe.Editors.Properties.Resources.normal_on;
			this.renderNormalsMenuItem.Name = "renderNormalsMenuItem";
			this.renderNormalsMenuItem.Size = new System.Drawing.Size(157, 22);
			this.renderNormalsMenuItem.Text = "Render normals";
			this.renderNormalsMenuItem.Click += new System.EventHandler(this.SelectNormalsOn);
			// 
			// hideNormalsMenuItem
			// 
			this.hideNormalsMenuItem.Image = global::Toe.Editors.Properties.Resources.normal_off;
			this.hideNormalsMenuItem.Name = "hideNormalsMenuItem";
			this.hideNormalsMenuItem.Size = new System.Drawing.Size(157, 22);
			this.hideNormalsMenuItem.Text = "Hide normals";
			this.hideNormalsMenuItem.Click += new System.EventHandler(this.SelectNormalsOff);
			// 
			// btnScreenShot
			// 
			this.btnScreenShot.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnScreenShot.Image = ((System.Drawing.Image)(resources.GetObject("btnScreenShot.Image")));
			this.btnScreenShot.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnScreenShot.Name = "btnScreenShot";
			this.btnScreenShot.Size = new System.Drawing.Size(23, 22);
			this.btnScreenShot.Text = "Take Screenshot";
			this.btnScreenShot.Click += new System.EventHandler(this.TakeScreenshot);
			// 
			// Base3DEditor
			// 
			this.Controls.Add(this.errPanel);
			this.Controls.Add(this.glControl);
			this.Controls.Add(this.toolStrip1);
			this.Name = "Base3DEditor";
			this.Size = new System.Drawing.Size(550, 450);
			this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.OnPreviewKeyDown);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.errPanel.ResumeLayout(false);
			this.errPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private void OnCameraPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.RefreshScene();
		}

		private void OnSceneGotFocus(object sender, EventArgs e)
		{
			if (this.cameraController != null)
			{
				this.cameraController.GotFocus();
			}
		}

		private void OnSceneLostFocus(object sender, EventArgs e)
		{
			if (this.cameraController != null)
			{
				this.cameraController.LostFocus();
			}
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

		private void RenderMeshNormals(IMesh mesh)
		{
			Color wireColor = Color.White;
			foreach (var submesh in mesh.Submeshes)
			{
				var pos = submesh.GetIndexReader(Streams.Position, 0);
				var normal = submesh.GetIndexReader(Streams.Normal, 0);
				var posStream = mesh.GetStreamReader<Vector3>(Streams.Position, 0);
				var normalStream = mesh.GetStreamReader<Vector3>(Streams.Normal, 0);
				if (posStream != null && normalStream != null && pos != null && normal != null)
				{
					for (int i = 0; i < pos.Count; i++)
					{
						this.graphicsContext.RenderDebugLine(posStream[pos[i]], posStream[pos[i]]+normalStream[normal[i]]*10, wireColor);
					}
				}
			}
		}

		private void RenderMeshWireframe(IMesh mesh)
		{
			var vb = mesh as IVertexStreamSource;
			var vertices = new List<Vector3>(vb.Count);
			Vector3 pos;
			Vector3 buf;
			var position = mesh.GetStreamReader<Vector3>(Streams.Position,0);
			for (int i = 0; i < position.Count; ++i)
			{
				pos = position[i];
				this.graphicsContext.ModelToWorld(ref pos, out buf);
				vertices.Add(buf);
			}
			Color wireColor = Color.White;

			foreach (var submesh in mesh.Submeshes)
			{
				var vi = submesh as IVertexIndexSource;
				var enumerator = vi.GetIndexReader(Streams.Position,0).GetEnumerator();
				switch (vi.VertexSourceType)
				{
					case VertexSourceType.TriangleList:
						for (;;)
						{
							if (!enumerator.MoveNext())
							{
								break;
							}
							var a = enumerator.Current;
							if (!enumerator.MoveNext())
							{
								break;
							}
							var b = enumerator.Current;
							if (!enumerator.MoveNext())
							{
								break;
							}
							var c = enumerator.Current;
							this.graphicsContext.RenderDebugLine(vertices[a], vertices[b], wireColor);
							this.graphicsContext.RenderDebugLine(vertices[b], vertices[c], wireColor);
							this.graphicsContext.RenderDebugLine(vertices[c], vertices[a], wireColor);
						}

						break;
					case VertexSourceType.TriangleStrip:
						break;
					case VertexSourceType.QuadList:
						for (;;)
						{
							if (!enumerator.MoveNext())
							{
								break;
							}
							var a = enumerator.Current;
							if (!enumerator.MoveNext())
							{
								break;
							}
							var b = enumerator.Current;
							if (!enumerator.MoveNext())
							{
								break;
							}
							var c = enumerator.Current;
							if (!enumerator.MoveNext())
							{
								break;
							}
							var d = enumerator.Current;
							this.graphicsContext.RenderDebugLine(vertices[a], vertices[b], wireColor);
							this.graphicsContext.RenderDebugLine(vertices[b], vertices[c], wireColor);
							this.graphicsContext.RenderDebugLine(vertices[c], vertices[d], wireColor);
							this.graphicsContext.RenderDebugLine(vertices[d], vertices[a], wireColor);
						}
						break;
					case VertexSourceType.QuadStrip:
						break;
					case VertexSourceType.LineLine:
						break;
					case VertexSourceType.LineStrip:
						break;
					default:
						break;
				}
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
			this.graphicsContext.SetViewport(0, 0, w, h);

			this.Camera.AspectRatio = w / (float)h;
		}

		private void TakeScreenshot(object sender, EventArgs e)
		{
			this.glControl.MakeCurrent();
			Bitmap bmp = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
			BitmapData data = bmp.LockBits(this.ClientRectangle, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
			GL.ReadPixels(
				0,
				0,
				this.ClientSize.Width,
				this.ClientSize.Height,
				OpenTK.Graphics.OpenGL.PixelFormat.Bgr,
				PixelType.UnsignedByte,
				data.Scan0);
			bmp.UnlockBits(data);
			bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

			var tmpPath = Path.GetTempPath();
			var dir = Path.Combine(tmpPath, "toe_screenshots");
			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}
			var t = DateTime.Now;
			var file = Path.Combine(
				dir, string.Format("{0}_{1}_{2}_{3}_{4}_{5}.jpg", t.Year, t.Month, t.Day, t.Hour, t.Minute, t.Second));
			bmp.Save(file);

			try
			{
				Process.Start("explorer.exe", "/select," + file);
			}
			catch (Exception ex)
			{
				Trace.WriteLine(ex);
			}
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
					this.coordinateSystemButton.Image = Properties.Resources.zup;
					break;
				case CoordinateSystem.YUp:
					this.coordinateSystemButton.Image = Properties.Resources.yup;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void UpdateLighingIcon()
		{
			if (this.options.Options.Lighting)
			{
				this.lightingButton.Image = Properties.Resources.light_on;
			}
			else
			{
				this.lightingButton.Image = Properties.Resources.light_off;
			}
		}

		private void errButton_Click(object sender, EventArgs e)
		{
			this.glControl.Visible = true;
			this.errPanel.Visible = false;
		}

		#endregion

		private void OnPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{

		}

		
	}
}