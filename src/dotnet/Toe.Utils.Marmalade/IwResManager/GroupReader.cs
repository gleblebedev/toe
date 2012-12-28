using System.IO;

using Autofac;

using Toe.Resources;

namespace Toe.Utils.Mesh.Marmalade.IwResManager
{
	public class GroupReader
	{
		private readonly IComponentContext context;

		public GroupReader(IComponentContext context)
		{
			this.context = context;
		}

		public Managed Parse(TextParser parser)
		{
			ResGroup group = new ResGroup(context.Resolve<IResourceManager>(), context) { BasePath = parser.BasePath };
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
				if (relPath.Length > 2 && relPath[0] == '.' && relPath[1] == Path.DirectorySeparatorChar) relPath = relPath.Substring(2);
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
							group.AddFile(fullPath);
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
						group.AddFile(fullPath);
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