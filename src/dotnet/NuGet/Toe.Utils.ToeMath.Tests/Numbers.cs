using System.Collections;
using System.Collections.Generic;

namespace Toe.Utils.ToeMath.Tests
{
	internal class Numbers:IEnumerable<int>
	{
		private readonly int to;

		private readonly int @from;

		public Numbers(int from,int to)
		{
			this.to = to;
			@from = @from;
		}

		#region Implementation of IEnumerable

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public IEnumerator<int> GetEnumerator()
		{
			for (int i = this.@from; i <= this.to; ++i) yield return i;
		}

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