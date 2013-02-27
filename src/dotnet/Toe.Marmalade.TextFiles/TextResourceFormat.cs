using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Autofac;

using Toe.Resources;
using Toe.Utils;

namespace Toe.Marmalade.TextFiles
{
	public class TextResourceFormat : IResourceFileFormat
	{
		#region Constants and Fields

		private static readonly string[] extensions = new[] { ".mtl", ".group", ".geo", ".anim", ".skel", ".skin", ".itx" };

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

		#region Public Properties

		/// <summary>
		/// List of extensions with leading dot.
		/// </summary>
		public IList<string> Extensions
		{
			get
			{
				return extensions;
			}
		}

		/// <summary>
		/// Name of file format.
		/// </summary>
		public string Name
		{
			get
			{
				return "Marmalade SDK Text Resource";
			}
		}

		#endregion

		#region Public Methods and Operators

		public bool CanRead(string filePath)
		{
			var f = filePath.ToLower();
			return extensions.Any(f.EndsWith);
		}

		public bool CanWrite(string filePath)
		{
			return false;
		}

		public IList<Managed> Load(Stream stream, string defaultName, IResourceFile resourceFile, string basePath)
		{
			IList<Managed> items = this.context.Resolve<IList<Managed>>();

			using (var source = new StreamReader(stream))
			{
				var parser = new TextParser(source, resourceFile, basePath);

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

					parser.UnknownLexemError();
				}
			}
		}

		public IList<IResourceFileItem> Read(string filePath, IResourceFile resourceFile)
		{
			var items = this.context.Resolve<IList<IResourceFileItem>>();

			using (var fileStream = File.OpenRead(filePath))
			{
				var resources = this.Load(
					fileStream,
					Path.GetFileNameWithoutExtension(filePath),
					resourceFile,
					Path.GetDirectoryName(Path.GetFullPath(filePath)));
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