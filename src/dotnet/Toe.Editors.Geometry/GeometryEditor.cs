using System.Drawing;
using System.IO;
using System.Windows.Forms;

using OpenTK.Graphics.OpenGL;

using Toe.Editors.Interfaces;
using Toe.Utils.Mesh;

namespace Toe.Editors.Geometry
{
	public class GeometryEditor : Base3DEditor, IResourceEditor
	{
		#region Constants and Fields

		private readonly IMeshReader meshReader;

		private readonly IMeshWriter meshWriter;

		private IMesh mesh;

		private uint vbo;

		private string currentFileName;

		#endregion

		#region Constructors and Destructors

		public GeometryEditor(IMeshReader meshReader, IMeshWriter meshWriter)
		{
			this.meshReader = meshReader;
			this.meshWriter = meshWriter;
		}

		#endregion

		public string CurrentFileName
		{
			get
			{
				return currentFileName;
			}
		}

		#region Public Properties

		public bool CanCopy
		{
			get
			{
				return false;
			}
		}

		public bool CanCut
		{
			get
			{
				return false;
			}
		}

		public bool CanPaste
		{
			get
			{
				return false;
			}
		}

		public bool CanRedo
		{
			get
			{
				return false;
			}
		}

		public bool CanUndo
		{
			get
			{
				return false;
			}
		}

		public Control Control
		{
			get
			{
				return this;
			}
		}

		#endregion

		#region Public Methods and Operators

		public void Delete()
		{
			throw new System.NotImplementedException();
		}

		public void LoadFile(string filename)
		{
			this.currentFileName = filename;
			using (var stream = File.OpenRead(filename))
			{
				this.mesh = this.meshReader.Load(stream);
			}
		}

		public void RecordCommand(string command)
		{
		}

		public void Redo()
		{
			
		}

		public void SaveFile(string filename)
		{
			if (this.meshWriter != null)
			{
				using (var stream = File.Create(filename))
				{
					this.meshWriter.Save(this.mesh, stream);
				}
			}
		}

		public void StopRecorder()
		{
		}

		public void Undo()
		{
			
		}

		public void onSelectAll()
		{
			
		}

		#endregion

		#region Methods

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			//if (vbo > 0)
			//{
			//    GL.DeleteBuffers(1, ref vbo);
			//    vbo = 0;
			//}
		}

		protected override void RenderScene()
		{
			GL.ClearColor(Color.SkyBlue);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			if (this.mesh != null)
			{
				this.mesh.RenderOpenGL();
			}
		}

		#endregion
	}
}