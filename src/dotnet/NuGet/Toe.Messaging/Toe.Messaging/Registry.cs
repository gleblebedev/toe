using System;
using System.Collections.Generic;
using System.Linq;

namespace Toe.Messaging
{
	public class Registry<T>
	{
		#region Constants and Fields

		protected T[] sortedValues;

		private readonly Func<T, int> getKey;

		#endregion

		#region Constructors and Destructors

		public Registry(IEnumerable<T> values, Func<T, int> getKey)
		{
			this.getKey = getKey;
			this.sortedValues = values.OrderBy(getKey).ToArray();
		}

		#endregion

		#region Methods

		protected T BinarySearch(int propertyType, int leftIndex, int rightIndex)
		{
			retry:
			if (rightIndex < leftIndex)
			{
				return default(T);
			}
			// calculate midpoint to cut set in half
			int midIndex = (leftIndex + rightIndex) >> 1;

			var key = this.getKey(this.sortedValues[midIndex]);

			// three-way comparison
			if (key > propertyType)
			{
				// key is in lower subset
				rightIndex = midIndex - 1;
				goto retry;
			}
			if (key < propertyType)
			{
				// key is in upper subset
				leftIndex = midIndex + 1;
				goto retry;
			}
			// key has been found
			return this.sortedValues[midIndex];
		}

		protected T BinarySearch(int propertyType)
		{
			return this.BinarySearch(propertyType, 0, this.sortedValues.Length - 1);
		}

		#endregion
	}
}