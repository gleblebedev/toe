using Toe.Utils.Mesh.Marmalade;

namespace Toe.Utils.Marmalade
{
	public interface IBinarySerializer
	{
		#region Public Properties

		/// <summary>
		/// Default file extension for binary resource file for this particular resource.
		/// </summary>
		string DefaultFileExtension { get; }

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Parse text block.
		/// </summary>
		Managed Parse(BinaryParser parser, string defaultName);

		#endregion
	}
}