using System.Collections.Generic;
using System.Linq;

namespace Toe.Scene
{
	public class SceneGraph
	{
		private SceneGraphRange[] ranges;

		private Entity[] entities;
		SceneGraph(IEnumerable<int> ranges)
		{
			this.ranges = new SceneGraphRange[ranges.Count()];
			this.entities = new Entity[ranges.Sum() + 1];
			int rangeIndex = 0;
			int min = 1;
			for (int index = 0; index < this.entities.Length; index++)
			{
				this.entities[index].id = index;
			}
			foreach (var range in ranges)
			{
				this.ranges[rangeIndex] = new SceneGraphRange(entities,min,range);
				min += range;
			}
		}
	}
}