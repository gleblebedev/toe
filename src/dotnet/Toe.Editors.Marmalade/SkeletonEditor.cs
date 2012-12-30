using OpenTK.Graphics.OpenGL;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Resources;
using Toe.Utils.Marmalade.IwAnim;
using Toe.Utils.Marmalade.IwGraphics;
using Toe.Utils.Mesh;
using Toe.Utils.Mesh.Marmalade.IwGx;

namespace Toe.Editors.Marmalade
{
	public class SkeletonEditor : Base3DEditor, IView
	{
		private readonly IEditorEnvironment editorEnvironment;

		private readonly IResourceManager resourceManager;

		private DataContextContainer dataContext = new DataContextContainer();
		#region Implementation of IView

		public AnimSkel Skeleton
		{
			get
			{
				if (this.dataContext.Value == null) return null;
				return (AnimSkel)this.dataContext.Value;
			}
			set
			{
				this.dataContext.Value = value;
			}
		}

		public DataContextContainer DataContext
		{
			get
			{
				return this.dataContext;
			}
		}

		#endregion

		public SkeletonEditor(IEditorEnvironment editorEnvironment, IResourceManager resourceManager)
		{
			this.editorEnvironment = editorEnvironment;
			this.resourceManager = resourceManager;
			this.InitializeComponent();

			this.InitializeEditor();
		}

		private void InitializeEditor()
		{

		}

		private void InitializeComponent()
		{

		}

		#region Overrides of Base3DEditor

		protected override void RenderScene()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			var model = Skeleton;
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

		#endregion
	}
}