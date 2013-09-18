using Toe.Resources;

namespace Toe.Marmalade.TextFiles
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
		void Serialize(TextSerializer parser, Managed managed);

		#endregion

	}
}