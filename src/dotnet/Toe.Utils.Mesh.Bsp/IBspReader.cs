using System.IO;

namespace Toe.Utils.Mesh.Bsp
{
	public interface IBspReader
	{
		#region Public Properties

		string GameRootPath { get; set; }

		Stream Stream { get; set; }

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Load generic scene from BSP file.
		/// </summary>
		/// <param name="stream"> </param>
		/// <returns>Loaded scene.</returns>
		IScene LoadScene();

		#endregion
	}
}