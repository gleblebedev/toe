using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Autofac;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Editors.Interfaces.Panels;
using Toe.Editors.Marmalade.Views;
using Toe.Gx;
using Toe.Marmalade.IwGraphics;
using Toe.Resources;
using Toe.Utils.Mesh;

namespace Toe.Editors.Marmalade
{
	public class ModelEditor : UserControl, IView
	{
		#region Constants and Fields

		private readonly IComponentContext context;

		private readonly DataContextContainer dataContext = new DataContextContainer();

		private readonly IEditorEnvironment editorEnvironment;

		private readonly ToeGraphicsContext graphicsContext;

		private readonly ICommandHistory history;

		private readonly IEditorOptions<Base3DEditorOptions> options;

		private readonly IResourceManager resourceManager;

		private Base3DEditor base3DEditor;

		private SplitContainer split;

		private StackPanel stackPanel1;

		private TableLayoutPanel tableLayoutPanel1;

		#endregion

		#region Constructors and Destructors

		public ModelEditor(
			IEditorEnvironment editorEnvironment,
			IResourceManager resourceManager,
			IComponentContext context,
			ToeGraphicsContext graphicsContext,
			IEditorOptions<Base3DEditorOptions> options,
			ICommandHistory history)
		{
			this.editorEnvironment = editorEnvironment;
			this.resourceManager = resourceManager;
			this.context = context;
			this.graphicsContext = graphicsContext;
			this.options = options;
			this.history = history;
			this.dataContext.DataContextChanged += ResetModel;
			this.InitializeComponent();

			this.InitializeEditor();
		}

		private void ResetModel(object sender, DataContextChangedEventArgs e)
		{
			if (this.Model == null)
				return;
			BoundingBox box = BoundingBox.Zero;
			foreach (var mesh in this.Model.Meshes)
			{
				foreach (var vertex in mesh.Vertices)
				{
					box = box.Union(vertex);
				}
			}
			if (!box.IsEmpty)
			{
				this.base3DEditor.CameraController.TargetDistance = Math.Min(1024, box.Size() / 2);
				this.base3DEditor.Camera.LookAt(box.Max * 1.5f, Vector3.Zero);
			}
		}

		#endregion

		#region Public Properties

		public DataContextContainer DataContext
		{
			get
			{
				return this.dataContext;
			}
		}

		public Model Model
		{
			get
			{
				if (this.dataContext.Value == null)
				{
					return null;
				}
				return (Model)this.dataContext.Value;
			}
			set
			{
				this.dataContext.Value = value;
			}
		}

		#endregion

		#region Methods

		protected void OnRenderScene(object sender, EventArgs args)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.PushAttrib(AttribMask.AllAttribBits);

			var model = this.Model;
			if (model != null)
			{
				this.graphicsContext.RenderModel(model);
			}
			this.graphicsContext.SetMaterial(null);
			GL.PopAttrib();
		}

		private void InitializeComponent()
		{
			this.split = new SplitContainer();
			this.stackPanel1 = new StackPanel();
			this.tableLayoutPanel1 = new TableLayoutPanel();
			var i = this.split as ISupportInitialize;
			if (i != null)
			{
				i.BeginInit();
			}
			this.split.Panel1.SuspendLayout();
			this.split.SuspendLayout();
			this.stackPanel1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// split
			// 
			this.split.Dock = DockStyle.Fill;
			this.split.Location = new Point(0, 0);
			this.split.Name = "split";
			// 
			// split.Panel1
			// 
			this.split.Panel1.Controls.Add(this.stackPanel1);
			this.split.Size = new Size(550, 150);
			this.split.SplitterDistance = 183;
			this.split.TabIndex = 0;
			// 
			// stackPanel1
			// 
			this.stackPanel1.AutoScroll = true;
			this.stackPanel1.Controls.Add(this.tableLayoutPanel1);
			this.stackPanel1.Dock = DockStyle.Fill;
			this.stackPanel1.Location = new Point(0, 0);
			this.stackPanel1.Name = "stackPanel1";
			this.stackPanel1.Size = new Size(183, 150);
			this.stackPanel1.TabIndex = 0;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
			this.tableLayoutPanel1.Location = new Point(3, 3);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
			this.tableLayoutPanel1.Size = new Size(183, 100);
			this.tableLayoutPanel1.TabIndex = 0;

			// 
			// ModelEditor
			// 
			this.Controls.Add(this.split);
			this.Name = "ModelEditor";
			this.Size = new Size(550, 150);
			this.split.Panel1.ResumeLayout(false);
			if (i != null)
			{
				i.EndInit();
			}
			this.split.ResumeLayout(false);
			this.stackPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);
		}

		private void InitializeEditor()
		{
			this.base3DEditor = this.context.Resolve<Base3DEditor>();
			this.split.Panel2.Controls.Add(this.base3DEditor);
			int row = 0;

			this.tableLayoutPanel1.Controls.Add(new Label { Text = "Name" }, 0, row);
			var editNameView = new EditNameView(this.history) { Dock = DockStyle.Fill };
			this.tableLayoutPanel1.Controls.Add(editNameView, 1, row);
			new DataContextBinding(editNameView, this.dataContext, false);

			++row;

			this.tableLayoutPanel1.Controls.Add(new Label { Text = "" }, 0, row);
			var importButton = new Button { Text = "Import" };
			importButton.Click += this.OnImportButtonClick;
			this.tableLayoutPanel1.Controls.Add(importButton, 1, row);

			this.base3DEditor.RenderScene += this.OnRenderScene;
		}

		private void OnImportButtonClick(object sender, EventArgs e)
		{
			var dialog = this.context.Resolve<ImportSceneDialog>();
			var scene = dialog.ImportScene();
			if (scene != null)
			{
			}
		}

		#endregion
	}
}