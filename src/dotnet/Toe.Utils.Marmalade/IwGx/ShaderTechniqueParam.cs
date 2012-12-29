using OpenTK.Graphics.OpenGL;

namespace Toe.Utils.Mesh.Marmalade.IwGx
{
	public abstract class ShaderTechniqueParam
	{
		private readonly string paramName;

		protected ShaderTechniqueParam(string paramName)
		{
			this.paramName = paramName;
		}

		public string ParamName
		{
			get
			{
				return this.paramName;
			}
		}

		public int Location { get; set; }

		public abstract void ApplyOpenGL(int shaderProgramHandle);
	}
}