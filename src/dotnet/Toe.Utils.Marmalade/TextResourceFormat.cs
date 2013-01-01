using System;
using System.Collections.Generic;
using System.IO;

using Autofac;

using Toe.Resources;

namespace Toe.Utils.Marmalade
{
	public class TextResourceFormat : IResourceFileFormat
	{
		#region Constants and Fields

		private readonly IComponentContext context;

		private readonly IResourceManager resourceManager;

		#endregion

		#region Constructors and Destructors

		public TextResourceFormat(IResourceManager resourceManager, IComponentContext context)
		{
			this.resourceManager = resourceManager;
			this.context = context;
		}

		#endregion

		#region Public Methods and Operators

		public bool CanRead(string filePath)
		{
			var f = filePath.ToLower();
			if (f.EndsWith(".mtl"))
			{
				return true;
			}
			if (f.EndsWith(".group"))
			{
				return true;
			}
			if (f.EndsWith(".geo"))
			{
				return true;
			}
			if (f.EndsWith(".anim"))
			{
				return true;
			}
			if (f.EndsWith(".skel"))
			{
				return true;
			}
			if (f.EndsWith(".skin"))
			{
				return true;
			}
			if (f.EndsWith(".itx"))
			{
				return true;
			}
			return false;
		}

		public bool CanWrite(string filePath)
		{
			return false;
		}

		public IList<Managed> Load(Stream stream, string defaultName, string basePath)
		{
			IList<Managed> items = this.context.Resolve<IList<Managed>>();

			using (var source = new StreamReader(stream))
			{
				var parser = new TextParser(source, basePath);

				for (;;)
				{
					var lexem = parser.Lexem;
					if (lexem == null)
					{
						return items;
					}
					object serializer;
					if (this.context.TryResolveKeyed(Hash.Get(lexem), typeof(ITextSerializer), out serializer))
					{
						items.Add(((ITextSerializer)serializer).Parse(parser, defaultName));
						continue;
					}

					parser.UnknownLexem();
				}
			}
			return items;
		}

		public IList<IResourceFileItem> Read(string filePath)
		{
			var items = this.context.Resolve<IList<IResourceFileItem>>();

			using (var fileStream = File.OpenRead(filePath))
			{
				var resources = this.Load(fileStream, Path.GetFileNameWithoutExtension(filePath), Path.GetDirectoryName(Path.GetFullPath(filePath)));
				foreach (var resource in resources)
				{
					items.Add(new ResourceFileItem(resource.ClassHashCode, resource));
				}
			}

			return items;
		}

		public void Write(string filePath, IList<IResourceFileItem> items)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}