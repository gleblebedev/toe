using System.Collections.Generic;
using System.IO;

namespace Toe.Utils.Mesh.Marmalade
{
	public class TextResourceWriter
	{
		public void Save(Stream stream, IEnumerable<Managed> items)
		{
			foreach (var managed in items)
			{
				throw new TextParserException("Unknown resource "+managed.GetType().Name);
			}
		}
	}
}