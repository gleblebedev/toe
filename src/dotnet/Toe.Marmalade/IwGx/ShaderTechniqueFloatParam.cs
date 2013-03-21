using Toe.Utils.Marmalade.IwGx;

namespace Toe.Marmalade.IwGx
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

		#region Public Properties

		public float[] Values
		{
			get
			{
				return this.values;
			}
		}

		#endregion
	}
}