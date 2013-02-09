using System.Collections.Generic;

namespace Toe.Core
{
	public class ToeMessageRegistry
	{
		readonly Dictionary<uint, int> map = new Dictionary<uint, int>();

		List<ToeMessageDescription> messages = new List<ToeMessageDescription>(1024);

		public void Register(ref ToeMessageDescription message)
		{
			if (!map.ContainsKey(message.Id))
			{
				map.Add(message.Id,messages.Count);
				messages.Add(message);
				return;
			}
			if (messages[map[message.Id]] != message) throw new ToeException(string.Format("Different version of message {0} registered already", message.Name));
		}
	}
}