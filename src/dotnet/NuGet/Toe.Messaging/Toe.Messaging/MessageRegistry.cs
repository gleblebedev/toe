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
				return new WritableMessageDescription(v, this.GetParentMessage(parentMessageId));
			}
			v = new MessageDescription { MessageId = messageId, ParentId = parentMessageId };
			this.map.Add(messageId, v);
			v.ReadOnlyDescription = null;
			return new WritableMessageDescription(v, this.GetParentMessage(parentMessageId));
		}

		public IMessageDescription GetDefinition(int messageId)
		{
			MessageDescription v;
			if (this.map.TryGetValue(messageId, out v))
			{
				return v.ReadOnlyDescription ?? (v.ReadOnlyDescription = new ReadOnlyMessageDescription(v));
			}
			return null;
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

			return new WritableMessageDescription(parent, this.GetParentMessage(parent.ParentId));
		}

		#endregion
	}
}