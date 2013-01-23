using OpenTK.Graphics.OpenGL;

using Toe.Marmalade;
using Toe.Marmalade.IwGx;

namespace Toe.Gx
{
	public class ShaderContext : IContextData
	{
		private readonly ShaderTechnique shaderTechnique;

		private int vs;

		private int fs;

		public ShaderContext(ShaderTechnique shaderTechnique)
		{
			this.shaderTechnique = shaderTechnique;
			vs = DefaultShaders.GenShader(ShaderType.VertexShader, shaderTechnique.VertexShaderSource);
			fs = DefaultShaders.GenShader(ShaderType.FragmentShader, shaderTechnique.FragmentShaderSource);
			indices = new ShaderTechniqueArgumentIndices();
			DefaultShaders.GenProgram(ref indices, vs, fs);
		}

		~ShaderContext()
		{
			this.Dispose(false);
		}

		private ShaderTechniqueArgumentIndices indices;

		public ShaderTechniqueArgumentIndices Indices
		{
			get
			{
				return this.indices;
			}
		}

		private void Dispose(bool managed)
		{
			
		}

		#region Implementation of IDisposable

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			this.Dispose(true);
		}

		#endregion
	}
}