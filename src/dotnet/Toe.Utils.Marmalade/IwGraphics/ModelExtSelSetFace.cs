using System.Collections.Generic;

namespace Toe.Utils.Mesh.Marmalade.IwGraphics
{
	public class ModelExtSelSetFace
	{
		#region Constants and Fields

		private readonly IList<int> f = new List<int>();

		#endregion

		#region Public Properties

		public IList<int> F
		{
			get
			{
				return this.f;
			}
		}

		public string Name { get; set; }

		public float otzOfs { get; set; }

		#endregion
	}
}