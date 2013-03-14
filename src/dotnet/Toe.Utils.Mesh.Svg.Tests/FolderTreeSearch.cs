using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Toe.Utils.Mesh.Svg.Tests
{
	internal class FolderTreeSearch : IEnumerable<string>
	{
		#region Constants and Fields

		private readonly string path;

		private readonly string searchPattern;

		#endregion

		#region Constructors and Destructors

		public FolderTreeSearch(string path, string searchPattern)
		{
			this.path = path;
			this.searchPattern = searchPattern;
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public IEnumerator<string> GetEnumerator()
		{
			if (!Directory.Exists(this.path))
			{
				yield break;
			}

			foreach (var fileName in Directory.GetFiles(this.path, this.searchPattern))
			{
				yield return fileName;
			}
			foreach (var dirName in Directory.GetDirectories(this.path))
			{
				foreach (var fileName in new FolderTreeSearch(dirName, this.searchPattern))
				{
					yield return fileName;
				}
			}
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
	}
}