using System.IO;

namespace Toe.Utils.Mesh.Marmalade
{
	public class BinaryParser
	{
		#region Constants and Fields

		private readonly string basePath;

		private readonly BinaryReader source;

		#endregion

		#region Constructors and Destructors

		public BinaryParser(BinaryReader source, string basePath)
		{
			this.source = source;
			this.basePath = basePath;
		}

		#endregion
	}
}