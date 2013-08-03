using System;
using System.Runtime.CompilerServices;

using Microsoft.CSharp.RuntimeBinder;

using Toe.CircularArrayQueue;

namespace Toe.Messaging
{
	public class DynamicSerializer : IMessageSerializer<object>
	{
		#region Constructors and Destructors

		public DynamicSerializer(MessageRegistry registry)
		{
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

		public void Deserialize(IMessageQueue queue, int pos, object value)
		{
			throw new NotImplementedException();
		}

		public void Serialize(IMessageQueue queue, object value)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}