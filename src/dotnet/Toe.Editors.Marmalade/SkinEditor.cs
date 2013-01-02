using OpenTK.Graphics.OpenGL;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Marmalade.IwAnim;
using Toe.Resources;
using Toe.Utils.Marmalade.IwAnim;
using Toe.Utils.Marmalade.IwGraphics;
using Toe.Utils.Mesh;

namespace Toe.Editors.Marmalade
{
	public class SkinEditor : Base3DEditor, IView
	{
		#region Constants and Fields

		private readonly DataContextContainer dataContext = new DataContextContainer();

		private readonly IEditorEnvironment editorEnvironment;

		private readonly IResourceManager resourceManager;

		#endregion

		#region Constructors and Destructors

		public SkinEditor(IEditorEnvironment editorEnvironment, IResourceManager resourceManager)
		{
			this.editorEnvironment = editorEnvironment;
			this.resourceManager = resourceManager;
			this.InitializeComponent();

			this.InitializeEditor();
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

		protected override void RenderScene()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			var skin = this.Skin;
			if (skin != null)
			{
				var skeleton = skin.Skeleton.Resource as AnimSkel;
				var model = skin.SkeletonModel.Resource as Model;
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
		}

		private void InitializeEditor()
		{
		}

		#endregion
	}
}