using System.IO;

using Autofac;

using Toe.Marmalade.IwResManager;
using Toe.Resources;

namespace Toe.Marmalade.TextFiles.IwResManager
{
	public class GroupTextSerializer : ITextSerializer
	{
		#region Constants and Fields

		private readonly IComponentContext context;

		#endregion

		#region Constructors and Destructors

		public GroupTextSerializer(IComponentContext context)
		{
			this.context = context;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Default file extension for text resource file for this particular resource.
		/// </summary>
		public string DefaultFileExtension
		{
			get
			{
				return ".group";
			}
		}

		#endregion

		#region Public Methods and Operators

		public Managed Parse(TextParser parser, string defaultName)
		{
			ResGroup group = new ResGroup(this.context.Resolve<IResourceManager>(), parser.ResourceFile, this.context)
				{ BasePath = parser.BasePath };
			group.Name = defaultName;
			parser.Consume("CIwResGroup");
			parser.Consume("{");
			for (;;)
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
					group.Name = parser.ConsumeString();
					continue;
				}
				if (attribute == "shared")
				{
					parser.Consume();
					group.IsShared = parser.ConsumeBool();
					continue;
				}
				if (attribute == "useTemplate")
				{
					//TODO: make a template handler
					parser.Consume();
					var ext = parser.ConsumeString();
					var name = parser.ConsumeString();
					continue;
				}

				var relPath = attribute.Replace('/', Path.DirectorySeparatorChar);
				if (relPath.Length > 2 && relPath[0] == '.' && relPath[1] == Path.DirectorySeparatorChar)
				{
					relPath = relPath.Substring(2);
				}
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
							ParseFileReference(parser, @group, fullPath);
							continue;
						}
						searchPath = Path.GetDirectoryName(searchPath);
					}
					while (!string.IsNullOrEmpty(searchPath));

					//fullPath = Path.Combine(searchPath, parser.BasePath);
					//ParseFileReference(parser, @group, fullPath);
					//continue;
				}
				else
				{
					fullPath = Path.Combine(parser.BasePath, relPath);
					//if (File.Exists(fullPath))
					{
						ParseFileReference(parser, @group, fullPath);
						continue;
					}
				}
				parser.UnknownLexemError();
			}
			return group;
		}

		#endregion

		#region Methods

		private static void ParseFileReference(TextParser parser, ResGroup @group, string fullPath)
		{
			@group.AddFile(fullPath);
			parser.ConsumeString();
			if (parser.Lexem == "{")
			{
				//TODO: make a block handler
				parser.ConsumeBlock();
			}
		}

		#endregion
	}
}