namespace Toe.Marmalade.BinaryFiles
{
	public interface IBinarySerializer
	{
		#region Public Methods and Operators

		/// <summary>
		/// Parse binary block.
		/// </summary>
		Managed Parse(BinaryParser parser);

		#endregion
	}
}