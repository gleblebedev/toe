using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Toe.Utils.Mesh
{
	public class DictionaryMeshStream<T> : IList<T>, IMeshStream
	{
		public DictionaryMeshStream(IStreamConverterFactory converterFactory)
		{
			this.converterFactory = converterFactory;
		}
		#region Constants and Fields

		private readonly List<T> list = new List<T>();

		private readonly Dictionary<T, int> map = new Dictionary<T, int>();

		private IStreamConverterFactory converterFactory;

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
				return this.list[index];
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

		#region Implementation of IMeshStream

		public IList<TValue> GetReader<TValue>()
		{
			if (typeof(TValue) == typeof(T))
				return new ReadOnlyCollection<TValue>((IList<TValue>)(object)this.list);
			if (ConverterFactory != null)
			{
				var resolveConverter = ConverterFactory.ResolveConverter<T, TValue>(this);
				if (resolveConverter != null)
					return resolveConverter;
			}
			throw new NotImplementedException(string.Format("{0} to {1} converter is not defined", typeof(T).FullName, typeof(TValue).FullName));
		}

		public IStreamConverterFactory ConverterFactory
		{
			get
			{
				return this.converterFactory;
			}
			set
			{
				this.converterFactory = value;
			}
		}

		#endregion
	}
}