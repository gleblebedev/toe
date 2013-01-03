using System;

using Autofac;

using Toe.Marmalade.IwAnim;
using Toe.Utils.Marmalade;

namespace Toe.Marmalade.BinaryFiles.IwAnim
{
	public class SkelBinarySerializer : IBinarySerializer
	{
		private readonly IComponentContext context;

		public SkelBinarySerializer(IComponentContext context)
		{
			this.context = context;
		}

		/// <summary>
		/// Parse binary block.
		/// </summary>
		public Managed Parse(BinaryParser parser)
		{
			throw new NotImplementedException();
		}


	}
}