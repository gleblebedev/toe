namespace Toe.Scene
{
	internal class SceneGraphRange
	{
		private readonly Entity[] entities;

		private readonly int left;

		private readonly int size;

		private int right;

		internal SceneGraphRange(Entity[] entities, int left, int size)
		{
			this.entities = entities;
			this.left = left;
			this.right = left + size-1;
			this.size = size;

			this.firstAvailableChild = left;
			this.lastAvailableChild = right;
			entities[this.firstAvailableChild].previousSibling = 0;
			entities[this.lastAvailableChild].nextSibling = 0;
			for (int i=left; i<right;++i)
			{
				entities[i].nextSibling = i + 1;
				entities[i + 1].previousSibling = i;
			}
			
		}
		public int Allocate()
		{
			var index = firstAvailableChild;
			if (index == 0) return 0;
			Detach(ref entities[index], index, ref firstAvailableChild, ref lastAvailableChild);
			AttachTail(ref entities[index], index, ref firstOccupiedChild, ref lastOccupiedChild);
			return entities[index].id;
		}
		public void Release(int id)
		{
			var index = id & 0x00FFFFFF;
			Detach(ref entities[index], index, ref firstOccupiedChild, ref lastOccupiedChild);
			AttachTail(ref entities[index], index, ref firstAvailableChild, ref lastAvailableChild);
		}

		private void AttachTail(ref Entity entity, int index, ref int first, ref int last)
		{
			if (last != 0)
			{
				entities[last].nextSibling = index;
			}
			else
			{
				first = index;
			}
			last = index;
			entity.previousSibling = last;
			entity.nextSibling = 0;
		}

		private void Detach(ref Entity entity, int index, ref int first, ref int last)
		{
			if (entity.previousSibling != 0)
			{
				entities[entity.previousSibling].nextSibling = entity.nextSibling;
			}
			else
			{
				first = entity.nextSibling;
			}
			if (entity.nextSibling != 0)
			{
				entities[entity.nextSibling].previousSibling = entity.previousSibling;
			}
			else
			{
				last = entity.previousSibling;
			}
		}

		private int firstAvailableChild;
		private int lastAvailableChild;
		private int firstOccupiedChild;
		private int lastOccupiedChild;
	}
}