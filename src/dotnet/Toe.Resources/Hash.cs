using System.Text;

namespace Toe.Resources
{
	public static class Hash
	{
		#region Constants and Fields

		public static uint HashInitial = 5381;

		#endregion

		#region Public Methods and Operators

		public static uint Get(byte[] str, uint hash)
		{
			if (str == null)
			{
				return 0;
			}

			foreach (var c in str)
			{
				var cc = (c < 'A' || c > 'Z') ? c : (c - 'A' + 'a'); // Ignore case!
				hash = (uint)(((hash << 5) + hash) + cc); // hash*33 + c
			}
			return hash;
		}

		public static uint Get(string str, uint hash)
		{
			if (string.IsNullOrEmpty(str))
			{
				return 0;
			}
			return Get(Encoding.UTF8.GetBytes(str), hash);
		}

		public static uint Get(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return 0;
			}
			return Get(Encoding.UTF8.GetBytes(str), HashInitial);
		}

		#endregion
	}
}