using System;

using Autofac;

using OpenTK.Graphics.OpenGL;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Gx;
using Toe.Marmalade.IwGx;
using Toe.Resources;

namespace Toe.Editors.Marmalade
{
	public class TextureEditor : Base3DEditor, IView
	{
		#region Constants and Fields

		private readonly DataContextContainer dataContext = new DataContextContainer();

		private readonly IEditorEnvironment editorEnvironment;

		private readonly ToeGraphicsContext graphicsContext;

		private readonly IResourceManager resourceManager;

		#endregion

		#region Constructors and Destructors

		public TextureEditor(
			IEditorEnvironment editorEnvironment,
			IResourceManager resourceManager,
			IComponentContext context,
			ToeGraphicsContext graphicsContext,
			IEditorOptions<Base3DEditorOptions> options,
			Base3DEditorContent content)
			: base(context, options, content)
		{
			this.editorEnvironment = editorEnvironment;
			this.resourceManager = resourceManager;
			this.graphicsContext = graphicsContext;
			this.InitializeComponent();

			this.InitializeEditor();
			base.RenderScene += this.RenderTexScene;
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

		public Texture Texture
		{
			get
			{
				if (this.dataContext.Value == null)
				{
					return null;
				}
				return (Texture)this.dataContext.Value;
			}
			set
			{
				this.dataContext.Value = value;
			}
		}

		#endregion

		#region Methods

		protected void RenderTexScene(object sender, EventArgs args)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.Color4(1.0f, 1.0f, 1.0f, 1.0f);
			var texture = this.Texture;
			if (texture != null)
			{
				this.graphicsContext.SetTexture(0, texture);
				GL.Enable(EnableCap.Blend);
				GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
				GL.Enable(EnableCap.AlphaTest);
				GL.AlphaFunc(AlphaFunction.Gequal, 0);
				base.RenderBox(250.0f);
				GL.Disable(EnableCap.Blend);
				GL.Disable(EnableCap.AlphaTest);
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