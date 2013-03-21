using System.Collections;
using System.Collections.Generic;

using Autofac;

using Toe.Utils;

namespace Toe.Marmalade
{
	public class ManagedList<T> : IEnumerable<T>
		where T : Managed
	{
		#region Constants and Fields

		private readonly IComponentContext context;

		private readonly List<T> listOfValues = new List<T>();

		#endregion

		#region Constructors and Destructors

		public ManagedList(IComponentContext context)
		{
			this.context = context;
		}

		#endregion

		#region Public Properties

		public int Capacity
		{
			get
			{
				return this.listOfValues.Capacity;
			}
			set
			{
				this.listOfValues.Capacity = value;
			}
		}

		public int Count
		{
			get
			{
				return this.listOfValues.Count;
			}
		}

		#endregion

		#region Public Indexers

		public T this[int i]
		{
			get
			{
				return this.listOfValues[i];
			}
		}

		#endregion

		#region Public Methods and Operators

		public int EnsureItem(string name)
		{
			var i = IndexOf(name);
			if (i < 0)
			{
				i = this.listOfValues.Count;
				var resolve = this.context.Resolve<T>();
				resolve.Name = name;
				this.listOfValues.Add(resolve);
			}
			return i;
		}

		public int EnsureItem(uint hash)
		{
			var i = IndexOf(hash);
			if (i < 0)
			{
				i = this.listOfValues.Count;
				var resolve = this.context.Resolve<T>();
				resolve.NameHash = hash;
				this.listOfValues.Add(resolve);
			}
			return i;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return this.listOfValues.GetEnumerator();
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

		#endregion

		#region Explicit Interface Methods

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

		#region Methods

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

		#endregion
	}
}