using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Autofac;
using Autofac.Core;

using Toe.Marmalade.IwResManager;
using Toe.Resources;

namespace Toe.Marmalade.BinaryFiles
{
	public class BinaryResourceFormat : IResourceFileFormat
	{
		#region Constants and Fields

		private const byte magic = 0x3D;

		private const byte major = 3;

		private const byte minor = 6;

		private static readonly string[] extensions = new[] { ".group.bin" };

		private readonly IComponentContext context;

		private readonly IResourceErrorHandler errorHandler;

		private readonly IResourceManager resourceManager;

		private byte rev = 1;

		#endregion

		#region Constructors and Destructors

		public BinaryResourceFormat(
			IResourceManager resourceManager, IComponentContext context, IResourceErrorHandler errorHandler)
		{
			this.resourceManager = resourceManager;
			this.context = context;
			this.errorHandler = errorHandler;
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
				return "Marmalade SDK Binary Group";
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

		public IList<Managed> Load(Stream stream, string basePath, IResourceFile resourceFile)
		{
			using (var source = new BinaryReader(stream))
			{
				var parser = new BinaryParser(source, basePath, resourceFile);
				return this.ParseGroupBin(parser);
			}
		}

		public IList<IResourceFileItem> Read(string filePath, IResourceFile resourceFile)
		{
			var items = this.context.Resolve<IList<IResourceFileItem>>();

			using (var fileStream = File.OpenRead(filePath))
			{
				var resources = this.Load(fileStream, Path.GetDirectoryName(Path.GetFullPath(filePath)), resourceFile);
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

		#region Methods

		private void ParseChildGroups(BinaryParser parser, ResGroup resGroup)
		{
			byte num = parser.ConsumeByte();
			while (num > 0)
			{
				string path = parser.ConsumeStringZ();
				if (!string.IsNullOrEmpty(path))
				{
					resGroup.AddFile(path);
				}
				{
					if (0 != parser.ConsumeUInt32())
					{
						throw new FormatException();
					}
				}
				{
					if (0x00001000 != parser.ConsumeInt32())
					{
						throw new FormatException();
					}
				}
				{
					if (0xd9794596 != parser.ConsumeUInt32())
					{
						throw new FormatException();
					}
				}

				--num;
			}
		}

		private IList<Managed> ParseGroupBin(BinaryParser parser)
		{
			IList<Managed> items = this.context.Resolve<IList<Managed>>();

			parser.Expect(magic);
			parser.Expect(major);
			parser.Expect(minor);
			this.rev = parser.ConsumeByte();

			if (0 != parser.ConsumeUInt16())
			{
				throw new FormatException();
			}

			var resGroup = this.context.Resolve<ResGroup>(new Parameter[] { TypedParameter.From(parser.ResourceFile) });
			resGroup.BasePath = parser.BasePath;
			items.Add(resGroup);

			for (;;)
			{
				var blockHash = parser.ConsumeUInt32();
				if (blockHash == 0)
				{
					break;
				}
				var pos = parser.Position;
				var len = parser.ConsumeUInt32();
				switch (blockHash)
				{
					case 0x8081E087:
						this.ParseResGroupMembers(parser, resGroup);
						break;
					case 0xDC3C2177:
						this.ParseGroupResources(parser, resGroup);
						break;
					case 0x3b495dc0:
						this.ParseChildGroups(parser, resGroup);
						break;
					default:
						throw new FormatException();
				}
				if (parser.Position != pos + len)
				{
					throw new FormatException();
				}
			}
			return items;
		}

		private void ParseGroupResources(BinaryParser parser, ResGroup resGroup)
		{
			uint numResources = parser.ConsumeUInt32();
			while (numResources > 0)
			{
				uint hash = parser.ConsumeUInt32();
				uint resCount = parser.ConsumeUInt32();

				bool unknown0 = parser.ConsumeBool();
				bool unknown1 = parser.ConsumeBool();

				while (resCount > 0)
				{
					--resCount;
					var pos = parser.Position;
					uint length = parser.ConsumeUInt32();

					object ser = null;
					if (!this.context.TryResolveKeyed(hash, typeof(IBinarySerializer), out ser))
					{
						this.errorHandler.CanNotRead(
							parser.BasePath, new FormatException(string.Format("Can't find resource reader for type {0}", hash)));
						parser.Skip(length - 4);
						continue;
					}
					var s = (IBinarySerializer)ser;
					Managed res = null;
					try
					{
						res = s.Parse(parser);
						resGroup.AddResource(res);
					}
					catch (Exception ex)
					{
						this.errorHandler.CanNotRead(
							parser.BasePath, new FormatException(string.Format("Can't read resource for type {0}", hash), ex));
						parser.Position = pos + length;
						continue;
					}

					if (parser.Position != pos + length)
					{
						throw new FormatException(
							string.Format(
								"Parse of {0} failed: wrong position by {1} bytes",
								(res == null) ? hash.ToString() : res.GetType().Name,
								parser.Position - (pos + length)));
						//parser.Position = pos + length;
					}
				}

				--numResources;
			}
		}

		private void ParseResGroupMembers(BinaryParser parser, ResGroup resGroup)
		{
			resGroup.Name = parser.ConsumeStringZ();
			resGroup.Flags = parser.ConsumeUInt32();
		}

		#endregion
	}
}