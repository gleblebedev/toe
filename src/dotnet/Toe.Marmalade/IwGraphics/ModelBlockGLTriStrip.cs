using Toe.Resources;
using Toe.Utils;

namespace Toe.Marmalade.IwGraphics
{
	public class ModelBlockGLTriStrip : ModelBlockGLPrimBase
	{
		#region Constants and Fields

		public static new readonly uint TypeHash = Hash.Get("ModelBlockGLTriStrip");

		#endregion

		#region Constructors and Destructors

		public ModelBlockGLTriStrip(IResourceManager resourceManager)
			: base(resourceManager)
		{
		}

		#endregion

		#region Public Properties

		public override uint ClassHashCode
		{
			get
			{
				return TypeHash;
			}
		}

		#endregion
	}
}