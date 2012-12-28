using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Toe.Resources;

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

		public static readonly uint TypeHash = Hash.Get("CIwModel");

		#region Overrides of Managed

		public override uint GetClassHashCode()
		{
			return TypeHash;
		}

		#endregion
	}
}
