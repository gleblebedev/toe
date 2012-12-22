using System.IO;

namespace Toe.Utils.Mesh
{
	public interface IMeshWriter
	{
		#region Public Methods and Operators

		void Save(IMesh mesh, Stream stream);

		#endregion
	}
}