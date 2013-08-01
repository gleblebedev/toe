using System;

namespace Toe.Messaging
{
	[AttributeUsage(AttributeTargets.Class)]
	public class MessageIdAttribute : Attribute
	{
		#region Constants and Fields

		private readonly int messageId;

		#endregion

		#region Constructors and Destructors

		public MessageIdAttribute(string name)
		{
			this.messageId = Hash.Eval(name);
		}

		public MessageIdAttribute(int id)
		{
			this.messageId = id;
		}

		#endregion

		#region Public Methods and Operators

		public static int Get(Type t)
		{
			var a = (MessageIdAttribute)GetCustomAttribute(t, typeof(MessageIdAttribute));
			if (a != null)
			{
				return a.messageId;
			}
			if (t.IsInterface && t.Name.StartsWith("I"))
			{
				return Hash.Eval(t.Name.Substring(1));
			}
			return Hash.Eval(t.Name);
		}

		#endregion
	}
}