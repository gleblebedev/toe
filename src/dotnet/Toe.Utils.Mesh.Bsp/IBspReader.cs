using System.IO;

namespace Toe.Utils.Mesh.Bsp
{
	public interface IBspReader
	{
		/// <summary>
		/// Load generic scene from BSP file.
		/// </summary>
		/// <param name="stream"> </param>
		/// <returns>Loaded scene.</returns>
		IScene LoadScene();

		Stream Stream { get; set; }

		string GameRootPath { get; set; }
	}
}