using System.Collections.Generic;
using System.Xml.Linq;

namespace Toe.Utils.Mesh.Dae
{
	/// <summary>
	/// Mesh skin and materials evaluated from nodes.
	/// </summary>
	public class MeshSkinAndMaterials
	{
		#region Constants and Fields

		public List<MaterialSet> Materials = new List<MaterialSet>();

		public XElement SkinController;

		#endregion

		#region Public Methods and Operators

		public IMaterial GetAnyMaterialFor(string value)
		{
			foreach (var materialSet in this.Materials)
			{
				var m = materialSet.Get(value);
				if (m != null)
				{
					return m;
				}
			}
			return null;
		}

		#endregion
	}

	public class MaterialSet
	{
		#region Constants and Fields

		private readonly Dictionary<string, IMaterial> map = new Dictionary<string, IMaterial>();

		#endregion

		#region Public Methods and Operators

		public void Add(string symbol, IMaterial material)
		{
			this.map[symbol] = material;
		}

		public IMaterial Get(string value)
		{
			IMaterial m;
			if (this.map.TryGetValue(value, out m))
			{
				return m;
			}
			return null;
		}

		#endregion
	}
}