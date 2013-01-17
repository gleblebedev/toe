using System;
using System.Collections.Generic;

namespace Toe.Utils.Mesh
{
	public delegate void ModifyAtFunc<T>(ref T item);
	public class MeshStream<T> : List<T>
	{
		#region Public Methods and Operators

		public void EnsureAt(int index)
		{
			while (this.Count <= index)
			{
				this.Add(default(T));
			}
		}

		#endregion

		public void ModifyAt (int index, ModifyAtFunc<T> func)
		{
			var v = this[index];
			func(ref v);
			this[index] = v;
		}
	}
}