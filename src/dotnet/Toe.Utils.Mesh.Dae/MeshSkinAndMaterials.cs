using System.Collections.Generic;
using System.Xml.Linq;

namespace Toe.Utils.Mesh.Dae
{
	/// <summary>
	/// Mesh skin and materials evaluated from nodes.
	/// </summary>
	public class MeshSkinAndMaterials
	{
		public XElement SkinController;
		public List<MaterialSet> Materials = new List<MaterialSet>();

		public IMaterial GetAnyMaterialFor(string value)
		{
			foreach (var materialSet in Materials)
			{
				var m = materialSet.Get(value);
				if (m != null) return m;
			}
			return null;
		}
	}

	public class MaterialSet
	{
		Dictionary<string,IMaterial> map = new Dictionary<string, IMaterial>();
		public void Add(string symbol, IMaterial material)
		{
			map[symbol] = material;
		}

		public IMaterial Get(string value)
		{
			IMaterial m;
			if (map.TryGetValue(value, out m)) return m;
			return null;
		}
	}
}