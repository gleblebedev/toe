using OpenTK.Graphics.OpenGL;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Resources;
using Toe.Utils.Mesh.Marmalade.IwGx;

namespace Toe.Editors.Marmalade
{
	public class TextureEditor : Base3DEditor, IView
	{
		private readonly IEditorEnvironment editorEnvironment;

		private readonly IResourceManager resourceManager;

		private DataContextContainer dataContext = new DataContextContainer();
		#region Implementation of IView

		public Texture Texture
		{
			get
			{
				if (this.dataContext.Value == null) return null;
				return (Texture)this.dataContext.Value;
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

		public TextureEditor(IEditorEnvironment editorEnvironment, IResourceManager resourceManager)
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

			GL.Color4(1.0f, 1.0f, 1.0f, 1.0f);
			var texture = Texture;
			if (texture != null)
			{
				texture.ApplyOpenGL(0);
				base.RenderBox(250.0f);
			}
		}

		#endregion
	}
}