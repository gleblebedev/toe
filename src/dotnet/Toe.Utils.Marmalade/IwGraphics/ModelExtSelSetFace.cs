using System.Collections.Generic;

namespace Toe.Utils.Mesh.Marmalade.IwGraphics
{
	public class ModelExtSelSetFace
	{
		public string Name { get; set; }

		public float otzOfs { get; set; }

		private readonly IList<int> f = new List<int>();

		public IList<int> F
		{
			get
			{
				return this.f;
			}
		}
	}
}