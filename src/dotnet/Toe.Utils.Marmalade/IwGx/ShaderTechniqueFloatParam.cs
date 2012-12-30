using OpenTK.Graphics.OpenGL;

using Toe.Gx;

namespace Toe.Utils.Mesh.Marmalade.IwGx
{
	public class ShaderTechniqueFloatParam : ShaderTechniqueParam
	{
		#region Constants and Fields

		private readonly float[] values;

		#endregion

		#region Constructors and Destructors

		public ShaderTechniqueFloatParam(string paramName, float[] values)
			: base(paramName)
		{
			this.values = values;
		}

		#endregion

		#region Public Methods and Operators

		public override void ApplyOpenGL(int shaderProgramHandle)
		{
			GL.Uniform1(shaderProgramHandle, this.Location, this.values);
			OpenTKHelper.Assert();
		}

		#endregion
	}
}