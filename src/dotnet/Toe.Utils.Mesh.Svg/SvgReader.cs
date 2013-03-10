using System;
using System.IO;

namespace Toe.Utils.Mesh.Svg
{
	public class SvgReader : ISceneReader
	{
		#region Implementation of ISceneReader

		/// <summary>
		/// Load mesh from stream.
		/// </summary>
		/// <param name="stream">Stream to read from.</param>
		/// <param name="basePath">Base path to resources.</param>
		/// <returns>Complete parsed mesh.</returns>
		public IScene Load(Stream stream, string basePath)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}