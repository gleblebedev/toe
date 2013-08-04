using System;

namespace Toe.Messaging
{
#if !WINDOWS_PHONE
	[Serializable]
#endif
	public class MessageRegistryException : Exception
	{
		#region Constructors and Destructors

		public MessageRegistryException(string propertyName)
			: base(string.Format("Mismatch description for property {0}", propertyName))
		{
		}

		#endregion
	}
}