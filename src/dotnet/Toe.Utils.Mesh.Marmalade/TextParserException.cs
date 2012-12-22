using System;

namespace Toe.Utils.Mesh.Marmalade
{
	[Serializable]
	public class TextParserException : Exception
	{
		#region Constructors and Destructors

		public TextParserException()
			: base()
		{
		}

		public TextParserException(string message)
			: base(message)
		{
		}

		public TextParserException(string message,Exception ex)
			: base(message,ex)
		{
		}

		#endregion
	}
}