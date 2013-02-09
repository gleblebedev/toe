using System.Collections;
using System.Collections.Generic;

using Autofac;

using Toe.Marmalade.IwAnim;
using Toe.Resources;
using Toe.Utils;

namespace Toe.Marmalade
{
	public class ManagedList<T> : IEnumerable<T>
		where T:Managed
	{
		private readonly IComponentContext context;

		readonly List<T> listOfValues = new List<T>();

		public ManagedList(IComponentContext context)
		{
			this.context = context;
		}

		public int Count
		{
			get
			{
				return listOfValues.Count;
			}
			
		}

		public int Capacity
		{
			get
			{
				return listOfValues.Capacity;
			}
			set
			{
				listOfValues.Capacity = value;
			}
		}

		public T this[int i]
		{
			get
			{
				return listOfValues[i];
			}
		}

		public int IndexOf(string boneName)
		{
			for (int index = 0; index < this.listOfValues.Count; index++)
			{
				var listOfValue = this.listOfValues[index];
				if (listOfValue.Name == boneName)
				{
					return index;
				}
			}
			return this.IndexOf(Hash.Get(boneName));
		}

		private int IndexOf(uint hash)
		{
			for (int index = 0; index < this.listOfValues.Count; index++)
			{
				var listOfValue = this.listOfValues[index];
				if (listOfValue.NameHash == hash)
				{
					return index;
				}
			}

			return -1;

		}

		public int EnsureItem(string name)
		{
			var i = IndexOf(name);
			if (i<0)
			{
				i = listOfValues.Count;
				var resolve = context.Resolve<T>();
				resolve.Name = name;
				listOfValues.Add(resolve);
			}
			return i;
		}

		public int EnsureItem(uint hash)
		{
			var i = IndexOf(hash);
			if (i < 0)
			{
				i = listOfValues.Count;
				var resolve = context.Resolve<T>();
				resolve.NameHash = hash;
				listOfValues.Add(resolve);
			}
			return i;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return listOfValues.GetEnumerator();
		}

		#region Implementation of IEnumerable

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}
}