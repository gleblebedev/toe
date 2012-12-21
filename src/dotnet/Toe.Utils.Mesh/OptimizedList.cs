using System.Collections;
using System.Collections.Generic;

namespace Toe.Utils.Mesh
{
	public class OptimizedList<T>: IList<T>
	{
		private readonly Dictionary<T, int> map = new Dictionary<T, int>();
		private readonly List<T> list = new List<T>();

		public IEnumerator<T> GetEnumerator()
		{
			return list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		void ICollection<T>.Add(T item)
		{
			this.Add(item);
		}

		public int Add(T item)
		{
			int index;
			if (map.TryGetValue(item, out index))
				return index;
			index = list.Count;
			list.Add(item);
			map[item] = index;
			return index;
		}

		public void Clear()
		{
			list.Clear();
			map.Clear();
		}

		public bool Contains(T item)
		{
			return map.ContainsKey(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			this.list.CopyTo(array,arrayIndex);
		}

		public bool Remove(T item)
		{
			throw new System.NotImplementedException();
		}

		public int Count
		{
			get { return list.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public int IndexOf(T item)
		{
			int index;
			if (map.TryGetValue(item, out index))
				return index;
			return -1;
		}

		public void Insert(int index, T item)
		{
			throw new System.NotImplementedException();
		}

		public void RemoveAt(int index)
		{
			throw new System.NotImplementedException();
		}

		public T this[int index]
		{
			get { return this[index]; }
			set { throw new System.NotImplementedException(); }
		}
	}
}