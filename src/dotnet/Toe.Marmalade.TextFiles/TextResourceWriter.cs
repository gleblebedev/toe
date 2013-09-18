using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Toe.Marmalade;
using Toe.Marmalade.TextFiles;
using Toe.Resources;
using Toe.Utils.TextParser;

namespace Toe.Utils.Marmalade
{
	public class TextResourceWriter
	{
		private readonly Func<uint, ITextSerializer> textSerializerFactory;

		public TextResourceWriter(Func<uint,ITextSerializer> textSerializerFactory)
		{
			this.textSerializerFactory = textSerializerFactory;
		}

		#region Public Methods and Operators

		public void Save(Stream stream, IEnumerable<IResourceFileItem> items, string basePath)
		{
			using (var writer = new StreamWriter(stream))
			{

				var serializer = new TextSerializer(writer, basePath);
				foreach (var managed in items)
				{
					var m = textSerializerFactory(managed.Type);
					if (m != null)
					{
						m.Serialize(serializer, managed.Resource as Managed);
					}
					else
					{
						throw new TextParserException(string.Format("Unknown resource {0}", managed.GetType().Name));
					}
				}
			}
		}

		#endregion
	}
}