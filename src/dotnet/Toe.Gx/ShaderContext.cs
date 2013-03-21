using OpenTK.Graphics.OpenGL;

using Toe.Marmalade;
using Toe.Marmalade.IwGx;

namespace Toe.Gx
{
	public class ShaderContext : IContextData
	{
		#region Constants and Fields

		private readonly int fs;

		private readonly ShaderTechniqueArgumentIndices indices;

		private readonly ShaderTechnique shaderTechnique;

		private readonly int vs;

		#endregion

		#region Constructors and Destructors

		public ShaderContext(ShaderTechnique shaderTechnique)
		{
			this.shaderTechnique = shaderTechnique;
			this.vs = DefaultShaders.GenShader(ShaderType.VertexShader, shaderTechnique.VertexShaderSource);
			this.fs = DefaultShaders.GenShader(ShaderType.FragmentShader, shaderTechnique.FragmentShaderSource);
			this.indices = new ShaderTechniqueArgumentIndices();
			DefaultShaders.GenProgram(ref this.indices, this.vs, this.fs);
		}

		~ShaderContext()
		{
			this.Dispose(false);
		}

		#endregion

		#region Public Properties

		public ShaderTechniqueArgumentIndices Indices
		{
			get
			{
				return this.indices;
			}
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

		#endregion

		#region Methods

		private void Dispose(bool managed)
		{
		}

		#endregion
	}
}