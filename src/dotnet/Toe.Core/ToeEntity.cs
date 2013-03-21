namespace Toe.Core
{
	/// <summary>
	/// Game entity.
	/// This class designed to produce as less garbage as possible.
	/// </summary>
	public struct ToeEntity
	{
		#region Constants and Fields

		/// <summary>
		/// Mask of available components.
		/// </summary>
		public uint ComponentMask;

		/// <summary>
		/// Child child node.
		/// </summary>
		public uint FirstChild;

		public ToeEntityId Id;

		/// <summary>
		/// Last child node.
		/// </summary>
		public uint LastChild;

		/// <summary>
		/// Next enitity in list of available/used/trashed.
		/// </summary>
		public uint Next;

		/// <summary>
		/// Next sibling.
		/// </summary>
		public uint NextSibling;

		/// <summary>
		/// Parent node.
		/// </summary>
		public uint Parent;

		/// <summary>
		/// Previous enitity in list of available/used/trashed.
		/// </summary>
		public uint Previous;

		/// <summary>
		/// Previous sibling.
		/// </summary>
		public uint PreviousSibling;

		#endregion
	}
}