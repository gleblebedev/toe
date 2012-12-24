using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

using Toe.Utils.Mesh.Marmalade.IwAnim;
using Toe.Utils.Mesh.Marmalade.IwGraphics;
using Toe.Utils.Mesh.Marmalade.IwGx;
using Toe.Utils.Mesh.Marmalade.IwResManager;

namespace Toe.Utils.Mesh.Marmalade
{
	public class TextResourceReader
	{
		public IList<Managed> Load(Stream stream, string basePath)
		{
			IList<Managed> items = new ObservableCollection<Managed>();

			using (var source = new StreamReader(stream))
			{
				var parser = new TextParser(source, basePath);

				for (; ; )
				{
					var lexem = parser.GetLexem();
					if (lexem == null) return items;
					if (lexem == "CIwMaterial")
					{
						items.Add(new MtlReader().Parse(parser));
						continue;
					}
					if (lexem == "CIwModel")
					{
						items.Add(new ModelReader().Parse(parser));
						continue;
					}
					if (lexem == "CMesh")
					{
						var streamMesh = new ModelReader().ParseMesh(parser);
						//items.Add(streamMesh);
						continue;
					}
					if (lexem == "CIwResGroup")
					{
						items.Add(new GroupReader().Parse(parser));
						continue;
					}
					if (lexem == "CIwAnimSkel")
					{
						var item = new SkelReader().Parse(parser);
						//items.Add(item);
						continue;
					}
					if (lexem == "CIwAnimSkin")
					{
						var item = new SkinReader().Parse(parser);
						//items.Add(item);
						continue;
					}
					parser.UnknownLexem();
				}
			}
			return items;
		}

	}
}