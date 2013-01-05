using System;

using Autofac;

using OpenTK.Graphics.OpenGL;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Resources;
using Toe.Utils.Marmalade.IwGraphics;

namespace Toe.Editors.Marmalade
{
	public class ModelEditor : Base3DEditor, IView
	{
		#region Constants and Fields

		private readonly DataContextContainer dataContext = new DataContextContainer();

		private readonly IEditorEnvironment editorEnvironment;

		private readonly IResourceManager resourceManager;

		#endregion

		#region Constructors and Destructors

		public ModelEditor(IEditorEnvironment editorEnvironment, IResourceManager resourceManager, IComponentContext context, IEditorOptions<Base3DEditorOptions> options):base(context,options)
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

		protected void RenderScene(object sender, EventArgs args)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.PushAttrib(AttribMask.AllAttribBits);

			GL.Enable(EnableCap.Lighting);
			GL.Enable(EnableCap.Light0);
			GL.Light(LightName.Light0, LightParameter.Position, new[] { this.Camera.Pos.X, this.Camera.Pos.Y, this.Camera.Pos.Z, 1.0f });

			var model = this.Model;
			if (model != null)
			{
				model.RenderOpenGL();
			}
			GL.PopAttrib();
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