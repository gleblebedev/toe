using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

using Autofac;

using Toe.Resources;
using Toe.Utils.Mesh.Marmalade.IwAnim;
using Toe.Utils.Mesh.Marmalade.IwGraphics;
using Toe.Utils.Mesh.Marmalade.IwGx;
using Toe.Utils.Mesh.Marmalade.IwResManager;

namespace Toe.Utils.Mesh.Marmalade
{
	public class TextResourceFormat : IResourceFileFormat
	{
		private readonly IResourceManager resourceManager;

		private readonly IComponentContext context;

		public TextResourceFormat(IResourceManager resourceManager,IComponentContext context)
		{
			this.resourceManager = resourceManager;
			this.context = context;
		}

		public IList<Managed> Load(Stream stream, string basePath)
		{
			IList<Managed> items = context.Resolve<IList<Managed>>();

			using (var source = new StreamReader(stream))
			{
				var parser = new TextParser(source, basePath);

				for (; ; )
				{
					var lexem = parser.GetLexem();
					if (lexem == null) return items;
					if (lexem == "CIwMaterial")
					{
						items.Add(new MtlReader(resourceManager).Parse(parser));
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
						items.Add(new GroupReader(context).Parse(parser));
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
					if (lexem == "CIwGxShaderTechnique")
					{
						throw new NotImplementedException("CIwGxShaderTechnique");
						//var item = new SkinReader().Parse(parser);
						//items.Add(item);
						continue;
					}
					parser.UnknownLexem();
				}
			}
			return items;
		}

		#region Implementation of IResourceFileReader

		public bool CanRead(string filePath)
		{
			var f = filePath.ToLower();
			if (f.EndsWith(".mtl")) return true;
			if (f.EndsWith(".group")) return true;
			if (f.EndsWith(".geo")) return true;
			if (f.EndsWith(".anim")) return true;
			if (f.EndsWith(".skel")) return true;
			if (f.EndsWith(".skin")) return true;
			if (f.EndsWith(".itx")) return true;
			return false;
		}

		public IList<IResourceFileItem> Read(string filePath)
		{
			var items = context.Resolve<IList<IResourceFileItem>>();

			using (var fileStream = File.OpenRead(filePath))
			{
				var resources = Load(fileStream, Path.GetDirectoryName(Path.GetFullPath(filePath)));
				foreach (var resource in resources)
				{
					items.Add(new ResourceFileItem(resource.GetClassHashCode(), resource));
				}
			}

			return items;
		}

		public bool CanWrite(string filePath)
		{
			return false;
		}

		public void Write(string filePath, IList<IResourceFileItem> items)
		{
			throw new System.NotImplementedException();
		}

		#endregion
	}
}