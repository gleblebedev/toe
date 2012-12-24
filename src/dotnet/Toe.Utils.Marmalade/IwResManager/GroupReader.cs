using System.IO;

namespace Toe.Utils.Mesh.Marmalade.IwResManager
{
	public class GroupReader
	{

		public Managed Parse(TextParser parser)
		{
			ResGroup group = new ResGroup();
			parser.Consume("CIwResGroup");
			parser.Consume("{");
			for (; ; )
			{
				var attribute = parser.GetLexem();
				if (attribute == "}")
				{
					parser.Consume();
					break;
				}
				if (attribute == "name")
				{
					parser.Consume();
					group.Name = parser.ConsumeString();
					continue;
				}
				var relPath = attribute.Replace('/', Path.DirectorySeparatorChar);
				string fullPath;
				if (relPath[0] == Path.DirectorySeparatorChar)
				{
					var searchPath = parser.BasePath;
					do
					{
						var subpath = relPath.Substring(1);
						fullPath = Path.Combine(searchPath, subpath);
						if (File.Exists(fullPath))
						{
							parser.ConsumeString();
							continue;
						}
						searchPath = Path.GetDirectoryName(searchPath);
					}
					while (true);


				}
				else
				{
					fullPath = Path.Combine(parser.BasePath, relPath);
					if (File.Exists(fullPath))
					{
						parser.ConsumeString();
						continue;
					}
				}
				parser.UnknownLexem();
			}
			return group;
		}
	}
}