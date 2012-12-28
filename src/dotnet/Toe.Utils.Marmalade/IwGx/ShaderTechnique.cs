using Toe.Resources;

namespace Toe.Utils.Mesh.Marmalade.IwGx
{
	public class ShaderTechnique:Managed
	{
		public static readonly uint TypeHash = Hash.Get("CIwGxShaderTechnique");

		#region Overrides of Managed

		public override uint GetClassHashCode()
		{
			return TypeHash;
		}

		#endregion
	}
}