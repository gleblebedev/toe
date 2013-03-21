using System;

namespace Toe.Core
{
	public class ToeException : Exception
	{
		#region Constructors and Destructors

		public ToeException(string message)
			: base(message)
		{
		}

		#endregion
	}
}