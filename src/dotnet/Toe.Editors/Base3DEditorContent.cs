using System;
using System.IO;
using System.Linq;

using OpenTK;

using Toe.Gx;
using Toe.Utils.Mesh;
using Toe.Utils.Mesh.Ase;

namespace Toe.Editors
{
	public class Base3DEditorContent : IDisposable
	{
		#region Constants and Fields

		private readonly IMesh cube;

		private readonly ToeGraphicsContext graphicsContext;

		private readonly IStreamConverterFactory streamConverterFactory;

		#endregion

		//private Texture cubeTex;

		#region Constructors and Destructors

		public Base3DEditorContent(ToeGraphicsContext graphicsContext, IStreamConverterFactory streamConverterFactory)
		{
			this.graphicsContext = graphicsContext;
			this.streamConverterFactory = streamConverterFactory;
			var cubeBytes = Properties.Resources.xyzcube;
			if (cubeBytes != null)
			{
				IScene scene = (new AseReader(streamConverterFactory)).Load(new MemoryStream(cubeBytes), null);
				foreach (var node in scene.Nodes)
				{
					if (node.Mesh != null)
					{
						this.cube = node.Mesh;
					}
				}
				//this.cubeTex = new Toe.Marmalade.IwGx.Texture ();
				//cubeTex.Image = new Toe.Marmalade.IwGx.Image (Toe.Editors.Properties.Resources.xyzcube1);
				this.cube.Submeshes.First().Material = new SceneMaterial
					{
						Effect =
							new SceneEffect
								{
									CullMode = CullMode.Front,
									Diffuse = new ImageColorSource { Image = new EmbeddedImage(Properties.Resources.xyzcube1) { } }
								}
					};
			}
		}

		~Base3DEditorContent()
		{
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			this.Dispose(true);
		}

		public void RenderXyzCube(GLControl glControl, EditorCamera camera)
		{
			if (this.cube != null)
			{
				//graphicsContext.SetMaterial(null);
				this.graphicsContext.DisableLighting();

				//this.graphicsContext.SetTexture(0, this.cubeTex);
				//this.graphicsContext.SetTexture(1, null);
				//this.graphicsContext.SetTexture(2, null);
				//this.graphicsContext.SetTexture(3, null);
				//GL.Enable(EnableCap.CullFace);
				//GL.CullFace(CullFaceMode.Front);
				OpenTKHelper.Assert();

				var result = new Matrix4();
				float ww = 384;
				var left = -ww / 2;
				var right = ww / 2;
				var bottom = -ww / 2;
				var top = ww / 2;
				var zFar = ww * 2;
				var zNear = -ww * 2;
				float invRL = 1 / (right - left);
				float invTB = 1 / (top - bottom);
				float invFN = 1 / (zFar - zNear);

				result.M11 = 2 * invRL;
				result.M22 = 2 * invTB;
				result.M33 = -2 * invFN;

				result.M41 = -(right + left) * invRL;
				result.M42 = -(top + bottom) * invTB;
				result.M43 = -(zFar + zNear) * invFN;
				result.M44 = 1;

				this.graphicsContext.SetProjection(ref result);

				int w = Math.Min(Math.Min(180, glControl.Width / 2), glControl.Height / 2);

				var pos = Vector3.Transform(new Vector3(0, 0, 200), camera.Rot);
				Matrix4 view = Matrix4.Rotate(camera.Rot) * Matrix4.CreateTranslation(pos);
				view.Invert();
				this.graphicsContext.SetView(ref view);

				this.graphicsContext.SetViewport(glControl.Width - w, glControl.Height - w, w, w);
				//GL.Viewport(glControl.Width - w, glControl.Height - w, w, w);

				this.graphicsContext.Render(this.cube);
				OpenTKHelper.Assert();
			}
		}

		#endregion

		#region Methods

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				//if (cubeTex != null)
				//{
				//    cubeTex.Dispose();
				//    cubeTex = null;
				//}
			}
		}

		#endregion
	}
}