using System;
using System.Collections.Generic;
using System.Linq;

namespace Toe.Utils.Mesh
{
	public class ListMeshStream<T> : List<T>, IMeshStream
	{
		#region Public Methods and Operators

		public void EnsureAt(int index)
		{
			while (this.Count <= index)
			{
				this.Add(default(T));
			}
		}
		public override string ToString()
		{
			if (base.Count > 0)
				return string.Format("{0}[{1}] {{ {2} ... }}", typeof(T).Name, base.Count, this.First());
			return string.Format("{0}[{1}]", typeof(T).Name, base.Count);
		}
		public void ModifyAt(int index, ModifyAtFunc<T> func)
		{
			var v = this[index];
			func(ref v);
			this[index] = v;
		}

		#endregion

		#region Implementation of IMeshStream

		public IList<TValue> GetReader<TValue>()
		{
			if (typeof(TValue) == typeof(T))
				return (IList<TValue>)this.AsReadOnly();
			throw new NotImplementedException();
		}


		#endregion

		public ListMeshStream()
		{
		}
		public ListMeshStream(int capacity):base(capacity)
		{
		}
	}
}