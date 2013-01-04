using System;

using OpenTK.Graphics.OpenGL;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Marmalade.IwAnim;
using Toe.Resources;
using Toe.Utils.Mesh;

namespace Toe.Editors.Marmalade
{
	public class SkeletonEditor : Base3DEditor, IView
	{
		#region Constants and Fields

		private readonly DataContextContainer dataContext = new DataContextContainer();

		private readonly IEditorEnvironment editorEnvironment;

		private readonly IResourceManager resourceManager;

		#endregion

		#region Constructors and Destructors

		public SkeletonEditor(IEditorEnvironment editorEnvironment, IResourceManager resourceManager)
		{
			this.editorEnvironment = editorEnvironment;
			this.resourceManager = resourceManager;
			this.InitializeComponent();

			this.InitializeEditor();
			base.RenderScene += this.RenderScene;
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

		public AnimSkel Skeleton
		{
			get
			{
				if (this.dataContext.Value == null)
				{
					return null;
				}
				return (AnimSkel)this.dataContext.Value;
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

			var model = this.Skeleton;
			if (model != null)
			{
				GL.Begin(BeginMode.Lines);
				GL.Color3(1.0f, 1.0f, 1.0f);
				model.Bones.UpdateAbsoluteValues();
				foreach (MeshBone bone in model.Bones)
				{
					var parent = bone.Parent;
					if (parent >= 0)
					{
						var parentBone = model.Bones[parent];
						GL.Vertex3(parentBone.AbsolutePos);
						GL.Vertex3(bone.AbsolutePos);
					}
				}
				GL.End();
			}
		}

		private void InitializeComponent()
		{
		}

		private void InitializeEditor()
		{
		}

		#endregion
	}
}