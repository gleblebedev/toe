using System;
using System.Drawing;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

#if WINDOWS_PHONE
#else

#endif

namespace Toe
{
	public partial class MainWindow : Form
	{
		#region Constants and Fields

		private readonly GLControl gl;

		private bool loaded;

		#endregion

		#region Constructors and Destructors

		public MainWindow()
		{
			this.gl = new GLControl(GraphicsMode.Default, 1, 0, GraphicsContextFlags.Default);

			this.gl.Dock = DockStyle.Fill;
			this.gl.Load += this.GLControlLoad;
			this.gl.Paint += this.GLControlPaint;
			this.gl.Resize += this.GLControlResize;
			this.Controls.Add(this.gl);
		}

		#endregion

		#region Methods

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
			GL.ClearColor(Color.SkyBlue);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			this.gl.SwapBuffers();
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

		private void SetupViewport()
		{
			int w = this.gl.Width;
			int h = this.gl.Height;
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(0, w, 0, h, -1, 1); // Bottom-left corner pixel has coordinate (0, 0)
			GL.Viewport(0, 0, w, h); // Use all of the glControl painting area
		}

		#endregion
	}
}