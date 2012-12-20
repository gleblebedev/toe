using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Toe
{
	public partial class MainWindow : Form
	{
		GLControl gl;
		bool loaded;
		public MainWindow (): base ()
		{
			gl = new GLControl(GraphicsMode.Default,1,0, GraphicsContextFlags.Default);
			gl.Dock = DockStyle.Fill;
			gl.Load += GLControlLoad;
			gl.Paint += GLControlPaint;
			gl.Resize += GLControlResize;
			this.Controls.Add(gl);
		}
		private void GLControlResize(object sender, EventArgs e)
		{
			if (!loaded)
				return;
			gl.MakeCurrent();
			SetupViewport();
		}
		
		private void GLControlPaint(object sender, PaintEventArgs e)
		{
			if (!loaded) // Play nice
				return;
			gl.MakeCurrent();
			GL.ClearColor(Color.SkyBlue);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			gl.SwapBuffers();
		}
		
		private void GLControlLoad(object sender, EventArgs e)
		{
			loaded = true;
			gl.MakeCurrent();
			SetupViewport();
		}
		
		
		private void SetupViewport()
		{
			int w = gl.Width;
			int h = gl.Height;
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(0, w, 0, h, -1, 1); // Bottom-left corner pixel has coordinate (0, 0)
			GL.Viewport(0, 0, w, h); // Use all of the glControl painting area
		}
	}
}
