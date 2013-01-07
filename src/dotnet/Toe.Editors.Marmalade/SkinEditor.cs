using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using Autofac;

using OpenTK.Graphics.OpenGL;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Editors.Interfaces.Panels;
using Toe.Editors.Interfaces.Views;
using Toe.Marmalade.IwAnim;
using Toe.Marmalade.IwGraphics;
using Toe.Resources;
using Toe.Utils.Marmalade.IwGraphics;
using Toe.Utils.Mesh;

namespace Toe.Editors.Marmalade
{
	public class SkinEditor : UserControl, IView, IBasePathProvider
	{
		#region Constants and Fields

		private readonly IComponentContext context;

		private readonly DataContextContainer dataContext = new DataContextContainer();

		private readonly IEditorEnvironment editorEnvironment;

		private readonly ICommandHistory history;

		private readonly IResourceManager resourceManager;

		private EditResourceReferenceView animButton;

		private Base3DEditor base3DEditor1;

		private SplitContainer split;

		private StackPanel stackPanel1;

		#endregion

		#region Constructors and Destructors

		public SkinEditor(
			IEditorEnvironment editorEnvironment,
			IResourceManager resourceManager,
			ICommandHistory history,
			IComponentContext context)
		{
			this.editorEnvironment = editorEnvironment;
			this.resourceManager = resourceManager;
			this.history = history;
			this.context = context;
			this.InitializeComponent();

			this.InitializeEditor();
			this.base3DEditor1.RenderScene += this.RenderScene;
		}

		#endregion

		#region Public Properties

		public string BasePath
		{
			get
			{
				if (this.Skin != null)
				{
					return this.Skin.BasePath;
				}
				return Directory.GetCurrentDirectory();
			}
		}

		public DataContextContainer DataContext
		{
			get
			{
				return this.dataContext;
			}
		}

		public AnimSkin Skin
		{
			get
			{
				if (this.dataContext.Value == null)
				{
					return null;
				}
				return (AnimSkin)this.dataContext.Value;
			}
			set
			{
				this.dataContext.Value = value;
			}
		}

		#endregion

		#region Methods

		protected void RenderScene(object sender, EventArgs args)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			var skin = this.Skin;
			if (skin != null)
			{
				var skeleton = skin.Skeleton.Resource as AnimSkel;
				var model = skin.SkeletonModel.Resource as Model;

				Anim anim = null;
				if (!this.animButton.Reference.IsEmpty)
				{
					anim = this.animButton.Reference.Resource as Anim;
				}

				if (anim != null && skeleton != null)
				{
					anim.Apply(skeleton, 0);
				}

				if (model != null)
				{
					GL.Enable(EnableCap.DepthTest);
					model.RenderOpenGL();
				}
				if (skeleton != null)
				{
					GL.Disable(EnableCap.DepthTest);
					GL.Begin(BeginMode.Lines);
					GL.Color3(1.0f, 1.0f, 1.0f);

					var boneCollection = skeleton.Bones;
					boneCollection.UpdateAbsoluteValues();
					foreach (MeshBone bone in boneCollection)
					{
						var parent = bone.Parent;
						if (parent >= 0)
						{
							var parentBone = boneCollection[parent];
							GL.Vertex3(parentBone.AbsolutePos);
							GL.Vertex3(bone.AbsolutePos);
						}
					}
					GL.End();
				}
			}
		}

		private void InitializeComponent()
		{
			ComponentResourceManager resources = new ComponentResourceManager(typeof(SkinEditor));
			this.split = new SplitContainer();
			this.stackPanel1 = new StackPanel();
			this.base3DEditor1 = new Base3DEditor(this.context, this.context.Resolve<IEditorOptions<Base3DEditorOptions>>(), this.context.Resolve<Base3DEditorContent>());
			var i = this.split as ISupportInitialize;
			if (i != null)
			{
				i.BeginInit();
			}
			this.split.Panel1.SuspendLayout();
			this.split.Panel2.SuspendLayout();
			this.split.SuspendLayout();
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
			// 
			// split.Panel2
			// 
			this.split.Panel2.Controls.Add(this.base3DEditor1);
			this.split.Size = new Size(750, 550);
			this.split.SplitterDistance = 250;
			this.split.TabIndex = 0;
			// 
			// stackPanel1
			// 
			this.stackPanel1.AutoScroll = true;
			this.stackPanel1.Dock = DockStyle.Fill;
			this.stackPanel1.Location = new Point(0, 0);
			this.stackPanel1.Name = "stackPanel1";
			this.stackPanel1.Size = new Size(250, 550);
			this.stackPanel1.TabIndex = 0;
			// 
			// base3DEditor1
			// 
			this.base3DEditor1.Dock = DockStyle.Fill;
			this.base3DEditor1.Location = new Point(0, 0);
			this.base3DEditor1.Name = "base3DEditor1";
			this.base3DEditor1.Size = new Size(496, 550);
			this.base3DEditor1.TabIndex = 0;
			// 
			// SkinEditor
			// 
			this.Controls.Add(this.split);
			this.Name = "SkinEditor";
			this.Size = new Size(750, 550);
			this.split.Panel1.ResumeLayout(false);
			this.split.Panel2.ResumeLayout(false);
			if (i != null)
			{
				i.EndInit();
			}
			this.split.ResumeLayout(false);
			this.ResumeLayout(false);
		}

		private void InitializeEditor()
		{
			this.stackPanel1.Controls.Add(new Label { Text = "Skeleton:" });
			var skel = new EditResourceReferenceView(
				this.editorEnvironment, this.resourceManager, this.history, this.context, false);
			new PropertyBinding<AnimSkin, ResourceReference>(skel, this.dataContext, a => a.Skeleton, null);
			this.stackPanel1.Controls.Add(skel);
			this.stackPanel1.Controls.Add(new Label { Text = "Model:" });
			var model = new EditResourceReferenceView(
				this.editorEnvironment, this.resourceManager, this.history, this.context, false);
			new PropertyBinding<AnimSkin, ResourceReference>(model, this.dataContext, a => a.SkeletonModel, null);
			this.stackPanel1.Controls.Add(model);
			this.stackPanel1.Controls.Add(new Label { Text = "Animation preview:" });
			this.animButton = new EditResourceReferenceView(
				this.editorEnvironment, this.resourceManager, this.history, this.context, true);
			this.animButton.DataContext.Value = new ResourceReference(Anim.TypeHash, this.resourceManager, this);
			this.stackPanel1.Controls.Add(this.animButton);
		}

		#endregion
	}
}