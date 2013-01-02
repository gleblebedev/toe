using System;

using Autofac;

using Toe.Utils.Marmalade;

namespace Toe.Marmalade.BinaryFiles.IwAnim
{
	public class SkinBinarySerializer : IBinarySerializer
	{
		private readonly IComponentContext context;

		public SkinBinarySerializer(IComponentContext context)
		{
			this.context = context;
		}

		/// <summary>
		/// Parse binary block.
		/// </summary>
		public Managed Parse(BinaryParser parser)
		{
			throw new NotImplementedException("Can't read skin");
		}


	}
}