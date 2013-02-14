using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using Autofac;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Gx;
using Toe.Utils.Mesh;

namespace Toe.Editors.GenericScene
{
	public class GenericSceneEditor : UserControl, IResourceEditor
	{
		private readonly IComponentContext context;

		private readonly ToeGraphicsContext graphicsContext;

		private readonly IEditorEnvironment editorEnvironment;

		private readonly ICommandHistory history;

		private readonly IEditorOptions<Base3DEditorOptions> options;

		private Base3DEditor base3DEditor;

		private string filePath;

		private IScene scene;


		public GenericSceneEditor(
			IEditorEnvironment editorEnvironment,
			IComponentContext context,
			ToeGraphicsContext graphicsContext,
			IEditorOptions<Base3DEditorOptions> options)
		{
			this.editorEnvironment = editorEnvironment;
			this.context = context;
			this.graphicsContext = graphicsContext;
			this.options = options;
			this.history = new CommandHistory();
			this.InitializeComponent();

			this.InitializeEditor();
		}

		private void InitializeEditor()
		{
			base3DEditor = new Base3DEditor(context, options, this.context.Resolve<Base3DEditorContent>());
			base3DEditor.Dock = DockStyle.Fill;
			this.Controls.Add(base3DEditor);
			base3DEditor.RenderScene += RenderEditorScene;
		}

		private void RenderEditorScene(object sender, EventArgs e)
		{
			GL.ClearColor(0.2f, 0.2f, 0.4f, 1f);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			if (scene != null)
			{
				graphicsContext.VsdProvider = scene.VsdProvider;
				Matrix4 parent = Matrix4.Identity;
				RenderSceneNodes(ref parent, scene.Nodes);
				graphicsContext.VsdProvider = null;
			}
		}

		private void RenderSceneNodes(ref Matrix4 parent, IList<INode> nodes)
		{
			if (nodes == null)
				return;
			foreach (var node in nodes)
			{
				Matrix4 modelMatrix = parent*node.ModelMatrix;
				this.RenderNodeAt(ref modelMatrix, node);
				this.RenderSceneNodes(ref modelMatrix, node.Nodes);
			}
		}

		private void RenderNodeAt(ref Matrix4 modelMatrix, INode node)
		{
			graphicsContext.SetModel(ref modelMatrix);

			if (node.Mesh != null)
				this.base3DEditor.RenderMesh(node.Mesh);

			Vector3 v;
			Vector3 center,x,y,z;
			v = new Vector3(0,0,0);
			Vector3.Transform(ref v, ref modelMatrix, out center);
			float size = 120;
			v = new Vector3(size, 0, 0);
			Vector3.Transform(ref v, ref modelMatrix, out x);
			v = new Vector3(0, size, 0);
			Vector3.Transform(ref v, ref modelMatrix, out y);
			v = new Vector3(0, 0, size);
			Vector3.Transform(ref v, ref modelMatrix, out z);
			Color c = Color.FromArgb(255, 255, 0, 0);
			graphicsContext.RenderDebugLine(ref center, ref x, ref c);
			c = Color.FromArgb(255, 0, 255, 0);
			graphicsContext.RenderDebugLine(ref center, ref y, ref c);
			c = Color.FromArgb(255, 0, 0, 255);
			graphicsContext.RenderDebugLine(ref center, ref z, ref c);
		}

		private void InitializeComponent()
		{
		}

		#region Implementation of IResourceEditor

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

		public string CurrentFileName
		{
			get
			{
				return this.filePath;
			}
		}

		public void Delete()
		{
			
		}

		public void LoadFile(string filename)
		{
			this.filePath = filename;
			var sceneFormats = context.Resolve<IEnumerable<ISceneFileFormat>>();
			foreach (var format in sceneFormats)
			{
				if (format.CanLoad(filename))
				{
					var reader = format.CreateReader();
					using (var s = File.OpenRead(filename))
					{
						scene = reader.Load(s);
					}
				}
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
	}
}