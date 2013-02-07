using System;
using System.Collections.Generic;
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
				Matrix4 parent = Matrix4.Identity;
				RenderSceneNodes(ref parent, scene.Nodes);
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
			this.base3DEditor.RenderMesh(node.Mesh);
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