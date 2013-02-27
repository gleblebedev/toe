using System.Collections.Generic;

namespace Toe.Utils.Mesh
{
	public class NodeSkin : INodeSkin
	{
		#region Constants and Fields

		private readonly Dictionary<string, IMaterialBinding> map = new Dictionary<string, IMaterialBinding>();

		#endregion

		#region Public Indexers

		public IMaterialBinding this[string taraget]
		{
			get
			{
				IMaterialBinding v;
				if (this.map.TryGetValue(taraget, out v))
				{
					return v;
				}
				v = new MaterialBinding(taraget);
				this.map.Add(taraget, v);
				return v;
			}
			set
			{
				this.map[taraget] = value;
			}
		}

		#endregion

		#region Public Methods and Operators

		public void RemoveBinding(string target)
		{
			this.map.Remove(target);
		}

		#endregion
	}
}