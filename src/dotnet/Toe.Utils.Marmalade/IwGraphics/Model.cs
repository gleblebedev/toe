using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Toe.Utils.Mesh.Marmalade.IwGraphics
{
	public class Model:Managed
	{

		public IList<IMesh> Meshes
		{
			get
			{
				return this.meshes;
			}
		}

		private readonly IList<IMesh> meshes = new List<IMesh>();

	}
}
