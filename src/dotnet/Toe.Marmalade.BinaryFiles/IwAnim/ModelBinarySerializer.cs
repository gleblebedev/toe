using System;
using System.Globalization;

using Autofac;

using Toe.Marmalade.IwGx;
using Toe.Utils.Marmalade;

namespace Toe.Marmalade.BinaryFiles.IwAnim
{
	public class AnimBinarySerializer:IBinarySerializer
	{
		private readonly IComponentContext context;

		public AnimBinarySerializer(IComponentContext context)
		{
			this.context = context;
		}

		/// <summary>
		/// Parse binary block.
		/// </summary>
		public Managed Parse(BinaryParser parser)
		{
			throw new NotImplementedException("Can't read anim");
		}

	
	}
}
