using Toe.Resources;

namespace Toe.Marmalade.IwGraphics
{
	public class ModelBlockGLTriList : ModelBlockGLPrimBase
	{
		public static new readonly uint TypeHash = Hash.Get("CIwModelBlockGLTriList");
		public override uint ClassHashCode
		{
			get
			{
				return TypeHash;
			}
		}
		public ModelBlockGLTriList(IResourceManager resourceManager)
			: base(resourceManager)
		{
		}
	}
}