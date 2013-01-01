using Toe.Utils.Mesh.Marmalade;

namespace Toe.Utils.Marmalade
{
	public interface ITextSerializer
	{
		#region Public Properties

		/// <summary>
		/// Default file extension for text resource file for this particular resource.
		/// </summary>
		string DefaultFileExtension { get; }

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Parse text block.
		/// </summary>
		Managed Parse(TextParser parser, string defaultName);

		#endregion
	}
}