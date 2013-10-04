using System;

namespace Toe.Utils.Mesh
{
	public class MeshStreamAttribute:Attribute
	{
		private readonly string key;

		private readonly int channel;

		public MeshStreamAttribute(string key)
		{
			this.key = key;
			this.channel = 0;
		}

		public MeshStreamAttribute(string key, int channel)
		{
			this.key = key;
			this.channel = channel;
		}
	}
}