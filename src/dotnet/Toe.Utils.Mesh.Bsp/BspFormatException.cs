using System;

namespace Toe.Utils.Mesh.Bsp
{
	public class BspFormatException : Exception
	{
		#region Constructors and Destructors

		public BspFormatException(string message)
			: base(message)
		{
		}

		#endregion
	}
}