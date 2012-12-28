using System.Text;

namespace Toe.Resources
{
	public static class Hash
	{
		public static uint HashInitial = 5381;

		public static uint Get(byte[] str, uint hash)
		{
			foreach (var c in str)
			{
				var cc = (c < 'A' || c > 'Z') ? c : (c - 'A' + 'a'); // Ignore case!
				hash = (uint)(((hash << 5) + hash) + cc);                // hash*33 + c
			}
			return hash;
		}

		public static uint Get(string str, uint hash)
		{
			return Get(Encoding.UTF8.GetBytes(str), hash);
		}

		public static uint Get(string str)
		{
			return Get(Encoding.UTF8.GetBytes(str), HashInitial);
		}
	}
}