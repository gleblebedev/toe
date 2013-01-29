namespace Toe.Core
{
	/// <summary>
	/// Game entity.
	/// This class designed to produce as less garbage as possible.
	/// </summary>
	public struct ToeEntity
	{
		#region Constants and Fields

		public ToeEntityId Id;

		#region Tree

		/// <summary>
		/// Parent node.
		/// </summary>
		public uint Parent;

		/// <summary>
		/// Child child node.
		/// </summary>
		public uint FirstChild;

		/// <summary>
		/// Last child node.
		/// </summary>
		public uint LastChild;

		/// <summary>
		/// Previous sibling.
		/// </summary>
		public uint PreviousSibling;

		/// <summary>
		/// Next sibling.
		/// </summary>
		public uint NextSibling;

		#endregion

		/// <summary>
		/// Next enitity in list of available/used/trashed.
		/// </summary>
		public uint Next;

		/// <summary>
		/// Previous enitity in list of available/used/trashed.
		/// </summary>
		public uint Previous;

		/// <summary>
		/// Mask of available components.
		/// </summary>
		public uint ComponentMask;

		#endregion
	}
}