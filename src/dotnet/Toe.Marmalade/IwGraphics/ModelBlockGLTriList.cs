using Toe.Resources;
using Toe.Utils;

namespace Toe.Marmalade.IwGraphics
{
	public class ModelBlockGLTriList : ModelBlockGLPrimBase
	{
		#region Constants and Fields

		public static new readonly uint TypeHash = Hash.Get("CIwModelBlockGLTriList");

		#endregion

		#region Constructors and Destructors

		public ModelBlockGLTriList(IResourceManager resourceManager)
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