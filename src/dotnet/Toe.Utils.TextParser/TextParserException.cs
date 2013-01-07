using System;
using System.Runtime.Serialization;

namespace Toe.Utils.TextParser
{
	[Serializable]
	public class TextParserException : Exception
	{
		#region Constructors and Destructors

		public TextParserException()
		{
		}

		public TextParserException(string message)
			: base(message)
		{
		}

		public TextParserException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public TextParserException(string message, Exception ex)
			: base(message, ex)
		{
		}

		#endregion
	}
}