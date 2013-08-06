using System;

namespace Toe.Messaging
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	public class MessageRegistryException : Exception
	{
		#region Constructors and Destructors

		public MessageRegistryException(PropertyDescription newDesc, PropertyDescription oldDesc)
			: base(string.Format("Mismatch description for property {0} / {1}", newDesc, oldDesc))
		{
		}

		#endregion
	}
}