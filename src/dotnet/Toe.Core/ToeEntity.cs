namespace Toe.Core
{
	/// <summary>
	/// Game entity.
	/// This class designed to produce as less garbage as possible.
	/// </summary>
	public struct ToeEntity
	{
		#region Constants and Fields

		public uint FirstChild;

		public uint Next;

		public uint NextChild;

		public uint NextSibling;

		public uint Parent;

		public uint Previous;

		public uint PreviousSibling;

		public uint UniqueId;

		#endregion
	}
}