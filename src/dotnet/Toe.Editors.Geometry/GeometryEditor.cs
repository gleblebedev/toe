using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;
using Toe.Editors.Interfaces;
using Toe.Utils.Mesh;

namespace Toe.Editors.Geometry
{
	public partial class GeometryEditor : UserControl, IResourceEditor
	{
		private readonly IMeshReader _meshReader;
		private GLControl gl;
		private bool loaded;

		public GeometryEditor(IMeshReader meshReader)
		{
			_meshReader = meshReader;
			InitializeComponent();

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


		public void StopRecorder()
		{
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

		public Control Control
		{
			get { return this; }
		}

		public void RecordCommand(string command)
		{
		}

		public void SaveFile(string fileName)
		{
		}

		public void LoadFile(string filename)
		{
			using (var stream = File.OpenRead(filename))
			{
				_meshReader.Load(stream);
			}
		}
	}
}
