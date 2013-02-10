using System.Collections.Generic;

namespace Toe.Utils.Mesh
{
	public class NodeSkin:INodeSkin
	{
		readonly Dictionary<string,IMaterialBinding> map = new Dictionary<string, IMaterialBinding>();

		#region Implementation of INodeSkin

		public IMaterialBinding this[string taraget]
		{
			get
			{
				IMaterialBinding v;
				if (map.TryGetValue(taraget, out v)) return v;
				v = new MaterialBinding(taraget);
				map.Add(taraget,v);
				return v;
			}
			set
			{
				map[taraget] = value;
			}
		}

		public void RemoveBinding(string target)
		{
			map.Remove(target);
		}

		#endregion
	}
}