using System;
using System.Runtime.CompilerServices;

using Microsoft.CSharp.RuntimeBinder;

using Toe.CircularArrayQueue;
using Toe.Messaging.Types;

namespace Toe.Messaging
{
	public class DynamicSerializer : IMessageSerializer<object>
	{
		#region Constants and Fields

		private readonly MessageRegistry registry;

		private readonly TypeRegistry typeRegistry;

		#endregion

		#region Constructors and Destructors

		public DynamicSerializer(MessageRegistry registry, TypeRegistry typeRegistry)
		{
			this.registry = registry;
			this.typeRegistry = typeRegistry;
		}

		#endregion

		#region Public Methods and Operators

		public static object GetProperty(object target, string name)
		{
			CallSite<Func<CallSite, object, object>> site =
				CallSite<Func<CallSite, object, object>>.Create(
					Binder.GetMember(
						CSharpBinderFlags.None,
						name,
						target.GetType(),
						new[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }));
			return site.Target(site, target);
		}

		public void Deserialize(IMessageQueue queue, int pos, dynamic value)
		{
			throw new NotImplementedException();
		}

		public void Serialize(IMessageQueue queue, dynamic value)
		{
			var messageIdValue = value.MessageId;
			int messageId;
			if (messageIdValue is int)
			{
				messageId = (int)messageIdValue;
			}
			else
			{
				messageId = Hash.Eval(messageIdValue.ToString());
			}
			var m = this.registry.GetDefinition(messageId);
			throw new NotImplementedException();
		}

		#endregion
	}
}