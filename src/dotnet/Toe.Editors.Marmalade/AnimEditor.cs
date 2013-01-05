using System;

using Autofac;

using OpenTK.Graphics.OpenGL;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Marmalade.IwAnim;
using Toe.Resources;
using Toe.Utils.Mesh;

namespace Toe.Editors.Marmalade
{
	public class AnimEditor : Base3DEditor, IView
	{
		#region Constants and Fields

		private readonly DataContextContainer dataContext = new DataContextContainer();

		private readonly IEditorEnvironment editorEnvironment;

		private readonly IResourceManager resourceManager;

		#endregion

		#region Constructors and Destructors

		public AnimEditor(IEditorEnvironment editorEnvironment, IResourceManager resourceManager, IComponentContext context, IEditorOptions<Base3DEditorOptions> options)
			: base(context,options)
		{
			this.editorEnvironment = editorEnvironment;
			this.resourceManager = resourceManager;
			this.InitializeComponent();

			this.InitializeEditor();
			base.RenderScene += this.RenderScene;
		}

		#endregion

		#region Public Properties

		public Anim Anim
		{
			get
			{
				if (this.dataContext.Value == null)
				{
					return null;
				}
				return (Anim)this.dataContext.Value;
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

		#region Methods

		protected void RenderScene(object sender, EventArgs args)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			var anim = this.Anim;
			if (anim != null)
			{
				var skeleton = anim.Skeleton.Resource as AnimSkel;

				if (skeleton != null)
				{
					GL.Disable(EnableCap.DepthTest);
					GL.Begin(BeginMode.Lines);
					GL.Color3(1.0f, 1.0f, 1.0f);

					var boneCollection = skeleton.Bones;
					anim.Apply(skeleton, 0);
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