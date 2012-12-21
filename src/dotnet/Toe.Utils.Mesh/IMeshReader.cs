using System.IO;

namespace Toe.Utils.Mesh
{
	public interface IMeshReader
	{
		IMesh Load(Stream stream);
	}
}