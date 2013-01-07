using System.Collections.Generic;
using System.IO;

using Toe.Marmalade;
using Toe.Utils.TextParser;

namespace Toe.Utils.Marmalade
{
	public class TextResourceWriter
	{
		#region Public Methods and Operators

		public void Save(Stream stream, IEnumerable<Managed> items)
		{
			foreach (var managed in items)
			{
				throw new TextParserException(string.Format("Unknown resource {0}", managed.GetType().Name));
			}
		}

		#endregion
	}
}