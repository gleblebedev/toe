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

		private readonly IResourceManager resourceManager;

		private readonly Func<uint, ITextDeserializer> textDeserializerFactory;

		private readonly Func<IList<IResourceFileItem>> itemsCollectionFactory;

		private readonly Func<IList<Managed>> managedCollectionFactory;

		#endregion

		#region Constructors and Destructors

		public TextResourceFormat(IResourceManager resourceManager, 
			Func<uint,ITextDeserializer> textDeserializerFactory,
			Func<IList<IResourceFileItem>> itemsCollectionFactory,
			Func<IList<Managed>> managedCollectionFactory)
		{
			this.resourceManager = resourceManager;
			this.textDeserializerFactory = textDeserializerFactory;
			this.itemsCollectionFactory = itemsCollectionFactory;
			this.managedCollectionFactory = managedCollectionFactory;
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
			IList<Managed> items = managedCollectionFactory();

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
					object serializer = textDeserializerFactory(Hash.Get(lexem));
					if (serializer != null)
					{
						items.Add(((ITextDeserializer)serializer).Parse(parser, defaultName));
						continue;
					}

					parser.UnknownLexemError();
				}
			}
		}

		public IList<IResourceFileItem> Read(string filePath, IResourceFile resourceFile)
		{
			var items = itemsCollectionFactory();

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