using System.Collections.Generic;

namespace Toe.Core
{
	public class ToeMessageRegistry
	{
		#region Constants and Fields

		private readonly Dictionary<uint, int> map = new Dictionary<uint, int>();

		private readonly List<ToeMessageDescription> messages = new List<ToeMessageDescription>(1024);

		#endregion

		#region Public Methods and Operators

		public void Register(ref ToeMessageDescription message)
		{
			if (!this.map.ContainsKey(message.Id))
			{
				this.map.Add(message.Id, this.messages.Count);
				this.messages.Add(message);
				return;
			}
			if (this.messages[this.map[message.Id]] != message)
			{
				throw new ToeException(string.Format("Different version of message {0} registered already", message.Name));
			}
		}

		#endregion
	}
}