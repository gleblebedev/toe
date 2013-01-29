using OpenTK;

using Toe.Utils.Marmalade.IwGx;

namespace Toe.Marmalade.IwGx
{
	public class ShaderTechniqueVec3Param : ShaderTechniqueParam
	{
		#region Constants and Fields

		private readonly Vector3[] values;

		#endregion

		#region Constructors and Destructors

		public ShaderTechniqueVec3Param(string paramName, Vector3[] values)
			: base(paramName)
		{
			this.values = values;
		}

		#endregion

		public Vector3[] Values
		{
			get
			{
				return this.values;
			}
		}
	}
}