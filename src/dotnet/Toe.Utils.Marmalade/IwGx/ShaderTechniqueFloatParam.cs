using OpenTK.Graphics.OpenGL;

using Toe.Gx;

namespace Toe.Utils.Mesh.Marmalade.IwGx
{
	public class ShaderTechniqueFloatParam : ShaderTechniqueParam
	{
		private readonly float[] values;

		public ShaderTechniqueFloatParam(string paramName, float[] values):base(paramName)
		{
			this.values = values;
		}

		public override void ApplyOpenGL(int shaderProgramHandle)
		{
			GL.Uniform1(shaderProgramHandle, Location, values);
			OpenTKHelper.Assert();
		}
	}
}