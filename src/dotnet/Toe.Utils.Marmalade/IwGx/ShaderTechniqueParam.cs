namespace Toe.Utils.Marmalade.IwGx
{
	public abstract class ShaderTechniqueParam
	{
		#region Constants and Fields

		private readonly string paramName;

		#endregion

		#region Constructors and Destructors

		protected ShaderTechniqueParam(string paramName)
		{
			this.paramName = paramName;
		}

		#endregion

		#region Public Properties

		public int Location { get; set; }

		public string ParamName
		{
			get
			{
				return this.paramName;
			}
		}

		#endregion

		#region Public Methods and Operators

		public abstract void ApplyOpenGL(int shaderProgramHandle);

		#endregion
	}
}