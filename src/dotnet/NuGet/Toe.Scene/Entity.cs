namespace Toe.Scene
{
	internal struct Entity
	{
		public int id;

		public int nextSibling;

		public int previousSibling;

		public int Index
		{
			get
			{
				return id & 0x00FFFFFF;
			}
		}


		public void IncrementVersion()
		{
			id += 1 << 24;
		}
	}
}