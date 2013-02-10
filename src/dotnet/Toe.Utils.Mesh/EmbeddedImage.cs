using System;

namespace Toe.Utils.Mesh
{
	public class EmbeddedImage : SceneItem, IImage
	{
		public string GetFilePath()
		{
			throw new InvalidOperationException();
		}

		public byte[] GetRawData()
		{
			throw new NotImplementedException();
		}
	}
}