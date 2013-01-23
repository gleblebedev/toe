using Toe.Resources;

namespace Toe.Marmalade.IwGraphics
{
	public class ModelBlockGLTriStrip : ModelBlockGLPrimBase
	{
		public static new readonly uint TypeHash = Hash.Get("ModelBlockGLTriStrip");
		public override uint ClassHashCode
		{
			get
			{
				return TypeHash;
			}
		}
		public ModelBlockGLTriStrip(IResourceManager resourceManager)
			: base(resourceManager)
		{
		}
	}
}