using System.Collections.Generic;

using Toe.Resources;
using Toe.Utils.Mesh;
using Toe.Utils.Mesh.Marmalade;

namespace Toe.Utils.Marmalade.IwGraphics
{
	public class Model : Managed
	{
		#region Constants and Fields

		public static readonly uint TypeHash = Hash.Get("CIwModel");

		private readonly IList<IMesh> meshes = new List<IMesh>();

		#endregion

		#region Public Properties

		public IList<IMesh> Meshes
		{
			get
			{
				return this.meshes;
			}
		}

		#endregion

		#region Public Methods and Operators

		public override uint GetClassHashCode()
		{
			return TypeHash;
		}

		#endregion
	}
}