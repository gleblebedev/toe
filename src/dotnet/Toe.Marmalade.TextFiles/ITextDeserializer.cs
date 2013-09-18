namespace Toe.Marmalade.TextFiles
{
	public interface ITextDeserializer
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