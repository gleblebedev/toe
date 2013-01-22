using System.Collections.Generic;

using OpenTK;

namespace Toe.Marmalade.IwGraphics.TangentSpace
{
	public class TangentMix
	{
		private readonly List<TangentMixItem> items = new List<TangentMixItem>();

		public List<TangentMixItem> Items
		{
			get
			{
				return this.items;
			}
		}

		public int IndexOf(ref Vector3 normal, ref Vector3 tangent)
		{
			foreach (var item in Items)
			{
				if (Vector3.Dot(item.Normal, normal) < 0.75f)
					continue;
				if (Vector3.Dot(item.Value, tangent) < 0.3f)
					continue;

				item.Value += tangent;
				item.Value.Normalize();

				return item.Index;
			}
			return -1;
		}

		public void Add(ref Vector3 normal, ref Vector3 tangent, int index)
		{
			Items.Add(new TangentMixItem(){Index = index, Normal = normal, Value = tangent});
		}
	}
}