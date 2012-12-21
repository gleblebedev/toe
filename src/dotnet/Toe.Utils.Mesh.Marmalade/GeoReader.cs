using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Toe.Utils.Mesh.Marmalade
{
	public class GeoReader: IMeshReader
	{
		public IMesh Load(Stream stream)
		{
			var mesh = new StreamMesh();

			using (var source = new StreamReader(stream))
			{
				var parser = new TextParser(source);

				parser.Consume("CIwModel");
				parser.Consume("{");

				for (; ; )
				{
					var attribute = parser.Lexem;
					if (attribute == "}")
					{
						parser.Consume();
						break;
					}
					if (attribute == "name")
					{
						parser.Consume();
						mesh.Name = parser.ConsumeString();
						continue;
					}
					if (attribute == "CMesh")
					{
						parser.Consume();
						parser.Consume("{");
						ParseMesh(parser, mesh);
						continue;
					}
					throw new TextParserException(string.Format("Unknown attribute {0}", attribute));
				}
			}

			return mesh;
		}

		private void ParseMesh(TextParser parser, StreamMesh mesh)
		{

			for (; ; )
			{
				var attribute = parser.Lexem;
				if (attribute == "}")
				{
					parser.Consume();
					break;
				}
				if (attribute == "name")
				{
					parser.Consume();
					mesh.Name = parser.ConsumeString();
					continue;
				}
				if (attribute == "scale")
				{
					parser.Consume();
					mesh.Scale = parser.ConsumeFloat();
					continue;
				}
				if (attribute == "CVerts")
				{
					parser.Consume();
					parser.Consume("{");
					ParseVerts(parser, mesh);
					continue;
				}
				throw new TextParserException(string.Format("Unknown attribute {0}", attribute));
			}
		}

		private void ParseVerts(TextParser parser, StreamMesh mesh)
		{
			throw new NotImplementedException();
		}
	}
}
