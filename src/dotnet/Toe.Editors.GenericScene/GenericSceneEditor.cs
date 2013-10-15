using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using Autofac;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Toe.Editors.Interfaces;
using Toe.Gx;
using Toe.Utils.Mesh;
using Toe.Utils.ToeMath;

namespace Toe.Editors.GenericScene
{
	public class GenericSceneEditor : UserControl, IResourceEditor
	{
		#region Constants and Fields

		private readonly IComponentContext context;

		private readonly IEditorEnvironment editorEnvironment;

		private readonly ToeGraphicsContext graphicsContext;

		private readonly ICommandHistory history;

		private readonly IEditorOptions<Base3DEditorOptions> options;

		private Base3DEditor base3DEditor;

		private string filePath;

		private IScene scene;

		#endregion

		#region Constructors and Destructors

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

		#endregion

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

		public string CurrentFileName
		{
			get
			{
				return this.filePath;
			}
		}

		#endregion

		#region Public Methods and Operators

		public void Delete()
		{
		}

		public void LoadFile(string filename)
		{
			this.filePath = filename;
			var sceneFormats = this.context.Resolve<IEnumerable<ISceneFileFormat>>();
			foreach (var format in sceneFormats)
			{
				if (format.CanLoad(filename))
				{
					var reader = format.CreateReader();
					using (var s = File.OpenRead(filename))
					{
						this.scene = reader.Load(s, Path.GetDirectoryName(Path.GetFullPath(filename)));
						this.base3DEditor.CameraController.Reset(this.scene.GetBoundingBox());
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

		#region Methods

		private void InitializeComponent()
		{
		}

		private void InitializeEditor()
		{
			this.base3DEditor = new Base3DEditor(this.context, this.options, this.context.Resolve<Base3DEditorContent>());
			this.base3DEditor.Dock = DockStyle.Fill;
			this.Controls.Add(this.base3DEditor);
			this.base3DEditor.RenderScene += this.RenderEditorScene;
		}

		private void RenderEditorScene(object sender, EventArgs e)
		{
			GL.ClearColor(0.2f, 0.2f, 0.4f, 1f);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			if (this.scene != null)
			{
				this.graphicsContext.VsdProvider = this.scene.VsdProvider;
				Matrix4 parent = Matrix4.Identity;
				this.RenderSceneNodes(ref parent, this.scene.Nodes);
				this.graphicsContext.VsdProvider = null;
			}
		}

		private void RenderNodeAt(ref Matrix4 modelMatrix, INode node)
		{
			this.graphicsContext.SetModel(ref modelMatrix);

			if (node.Mesh != null)
			{
				this.base3DEditor.RenderMesh(node.Mesh);
			}

			Vector3 v;
			Vector3 center, x, y, z;
			v = new Vector3(0, 0, 0);
			Vector3.Transform(ref v, ref modelMatrix, out center);
			float size = 120;
			v = new Vector3(size, 0, 0);
			Vector3.Transform(ref v, ref modelMatrix, out x);
			v = new Vector3(0, size, 0);
			Vector3.Transform(ref v, ref modelMatrix, out y);
			v = new Vector3(0, 0, size);
			Vector3.Transform(ref v, ref modelMatrix, out z);
			Color c = Color.FromArgb(255, 255, 0, 0);
			this.graphicsContext.RenderDebugLine(ref center, ref x, ref c);
			c = Color.FromArgb(255, 0, 255, 0);
			this.graphicsContext.RenderDebugLine(ref center, ref y, ref c);
			c = Color.FromArgb(255, 0, 0, 255);
			this.graphicsContext.RenderDebugLine(ref center, ref z, ref c);
		}

		private void RenderSceneNodes(ref Matrix4 parent, IList<INode> nodes)
		{
			if (nodes == null)
			{
				return;
			}
			foreach (var node in nodes)
			{
				Matrix4 modelMatrix = parent * node.ModelMatrix.ToMatrix();
				this.RenderNodeAt(ref modelMatrix, node);
				this.RenderSceneNodes(ref modelMatrix, node.Nodes);
			}
		}

		#endregion
	}
}