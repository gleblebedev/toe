using System.Collections.Generic;
using System.IO;

using Toe.Utils.Mesh.Marmalade.IwGx;

namespace Toe.Utils.Mesh.Marmalade
{
	public class TextResourceReader
	{
		public IList<Managed> Load(Stream stream)
		{
			IList<Managed> items = new List<Managed>();

			using (var source = new StreamReader(stream))
			{
				var parser = new TextParser(source);

				for (; ; )
				{
					var lexem = parser.GetLexem();
					if (lexem == null) return items;
					if (lexem == "CIwMaterial")
					{
						items.Add(new MtlReader().Parse(parser));
						continue;
					}
					parser.UnknownLexem();
				}
			}
			return items;
		}

	}
}