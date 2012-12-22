using System;
using System.Collections;
using System.Collections.Generic;

namespace Toe.Utils.Mesh
{
	public class OptimizedList<T> : IList<T>
	{
		#region Constants and Fields

		private readonly List<T> list = new List<T>();

		private readonly Dictionary<T, int> map = new Dictionary<T, int>();

		#endregion

		#region Public Properties

		public int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		#endregion

		#region Public Indexers

		public T this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		#endregion

		#region Public Methods and Operators

		public int Add(T item)
		{
			int index;
			if (this.map.TryGetValue(item, out index))
			{
				return index;
			}
			index = this.list.Count;
			this.list.Add(item);
			this.map[item] = index;
			return index;
		}

		public void Clear()
		{
			this.list.Clear();
			this.map.Clear();
		}

		public bool Contains(T item)
		{
			return this.map.ContainsKey(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			this.list.CopyTo(array, arrayIndex);
		}

		public IEnumerator<T> GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		public int IndexOf(T item)
		{
			int index;
			if (this.map.TryGetValue(item, out index))
			{
				return index;
			}
			return -1;
		}

		public void Insert(int index, T item)
		{
			throw new NotImplementedException();
		}

		public bool Remove(T item)
		{
			throw new NotImplementedException();
		}

		public void RemoveAt(int index)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Explicit Interface Methods

		void ICollection<T>.Add(T item)
		{
			this.Add(item);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}
}