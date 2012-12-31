using OpenTK.Graphics.OpenGL;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Resources;
using Toe.Utils.Marmalade.IwGraphics;
using Toe.Utils.Mesh;
using Toe.Utils.Mesh.Marmalade.IwGraphics;
using Toe.Utils.Mesh.Marmalade.IwGx;

namespace Toe.Editors.Marmalade
{
	public class ModelEditor : Base3DEditor, IView
	{
		private readonly IEditorEnvironment editorEnvironment;

		private readonly IResourceManager resourceManager;

		private DataContextContainer dataContext = new DataContextContainer();
		#region Implementation of IView

		public Model Model
		{
			get
			{
				if (this.dataContext.Value == null) return null;
				return (Model)this.dataContext.Value;
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

		public ModelEditor(IEditorEnvironment editorEnvironment, IResourceManager resourceManager)
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

			GL.PushAttrib(AttribMask.AllAttribBits);

			GL.Enable(EnableCap.Lighting);
			GL.Enable(EnableCap.Light0);
			GL.Light(LightName.Light0, LightParameter.Position, new float[] { Camera.Pos.X, Camera.Pos.Y, Camera.Pos.Z });

			var model = Model;
			if (model != null)
			{
				model.RenderOpenGL();
				
			}
			GL.PopAttrib();
		}

		#endregion
	}
}