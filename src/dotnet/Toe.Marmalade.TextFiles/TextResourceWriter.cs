using System.Collections.Generic;
using System.IO;

using Toe.Marmalade;

namespace Toe.Utils.Marmalade
{
	public class TextResourceWriter
	{
		#region Public Methods and Operators

		public void Save(Stream stream, IEnumerable<Managed> items)
		{
			foreach (var managed in items)
			{
				throw new TextParserException("Unknown resource " + managed.GetType().Name);
			}
		}

		#endregion
	}
}