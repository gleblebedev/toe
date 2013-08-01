using System;
using System.Collections.Generic;

namespace Toe.Messaging
{
	public class MessageRegistry
	{
		#region Constants and Fields

		private readonly Dictionary<int, MessageDescription> map = new Dictionary<int, MessageDescription>();

		#endregion

		#region Public Methods and Operators

		public IMessageDescription DefineMessage(int messageId, int parentMessageId = 0)
		{
			MessageDescription v;
			if (this.map.TryGetValue(messageId, out v))
			{
				if (v.ParentId != parentMessageId)
				{
					throw new ArgumentException("Different parent message");
				}
				return new ExistingMessageDescription(v, this.GetParentMessage(parentMessageId));
			}
			v = new MessageDescription { MessageId = messageId, ParentId = parentMessageId };
			return new NewMessageDescription(v, this.GetParentMessage(parentMessageId));
		}

		#endregion

		#region Methods

		private IMessageDescription GetParentMessage(int parentMessageId)
		{
			MessageDescription parent = null;
			if (parentMessageId == 0)
			{
				return null;
			}

			if (!this.map.TryGetValue(parentMessageId, out parent))
			{
				throw new ArgumentException("Unknown parent message");
			}

			return new ExistingMessageDescription(parent, this.GetParentMessage(parent.ParentId));
		}

		#endregion
	}
}