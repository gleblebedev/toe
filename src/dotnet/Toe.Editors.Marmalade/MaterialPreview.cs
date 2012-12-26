using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Toe.Editors.Marmalade
{
	public class MaterialPreview:Base3DEditor
	{
		#region Overrides of Base3DEditor

		protected override void RenderScene()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			RenderBox(50);
		}

		private void RenderBox(float size)
		{
			Vector3[] p = new Vector3[]
				{
					
new Vector3( -50,-50,-50 ),
new Vector3( 50,-50,-50 ),
new Vector3( -50,50,-50 ),
new Vector3( 50,50,-50 ),
new Vector3( -50,-50,50 ),
new Vector3( 50,-50,50 ),
new Vector3( -50,50,50 ),
new Vector3( 50,50,50 ),

				};
			GL.Begin(BeginMode.Triangles);

			GL.Vertex3(p[0]);
			GL.Vertex3(p[1]);
			GL.Vertex3(p[3]);

			GL.Vertex3(p[0]);
			GL.Vertex3(p[3]);
			GL.Vertex3(p[2]);

			GL.Vertex3(p[4]);
			GL.Vertex3(p[5]);
			GL.Vertex3(p[7]);

			GL.Vertex3(p[4]);
			GL.Vertex3(p[7]);
			GL.Vertex3(p[6]);

			GL.End();
		}

		#endregion
	}
}