using System;
using System.Runtime.CompilerServices;

using Microsoft.CSharp.RuntimeBinder;

using Toe.CircularArrayQueue;

namespace Toe.Messaging
{
	public class DynamicSerializer : IMessageSerializer<object>
	{
		public DynamicSerializer(MessageRegistry registry)
		{
			
		}

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


		#region Implementation of IMessageSerializer

		public int Serialize(IMessageQueue queue, object value)
		{
			throw new NotImplementedException();
		}

		public void Deserialize(IMessageQueue queue, int pos, object value)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}