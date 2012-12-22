using System.Collections.Generic;

namespace Toe.Utils.Mesh
{
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
	}
}