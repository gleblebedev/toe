using System;
using System.Globalization;

using Autofac;

using Toe.Marmalade.IwGx;
using Toe.Utils.Marmalade;
using Toe.Utils.Marmalade.IwGraphics;

namespace Toe.Marmalade.BinaryFiles.IwGraphics
{
	public class ModelBinarySerializer:IBinarySerializer
	{
		private readonly IComponentContext context;

		public ModelBinarySerializer(IComponentContext context)
		{
			this.context = context;
		}

		/// <summary>
		/// Parse binary block.
		/// </summary>
		public Managed Parse(BinaryParser parser)
		{
			throw new NotImplementedException("Can't read model");
		}

	

	}
}
