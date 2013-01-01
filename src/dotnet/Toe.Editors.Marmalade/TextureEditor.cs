using OpenTK.Graphics.OpenGL;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Resources;
using Toe.Utils.Marmalade.IwGx;

namespace Toe.Editors.Marmalade
{
	public class TextureEditor : Base3DEditor, IView
	{
		#region Constants and Fields

		private readonly DataContextContainer dataContext = new DataContextContainer();

		private readonly IEditorEnvironment editorEnvironment;

		private readonly IResourceManager resourceManager;

		#endregion

		#region Constructors and Destructors

		public TextureEditor(IEditorEnvironment editorEnvironment, IResourceManager resourceManager)
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

		protected override void RenderScene()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.Color4(1.0f, 1.0f, 1.0f, 1.0f);
			var texture = this.Texture;
			if (texture != null)
			{
				texture.ApplyOpenGL(0);
				base.RenderBox(250.0f);
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