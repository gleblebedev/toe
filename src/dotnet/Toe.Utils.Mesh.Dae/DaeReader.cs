using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Toe.Utils.Mesh.Dae
{
	public class DaeReader : IMeshReader
	{
		#region Implementation of IMeshReader

		/// <summary>
		/// Load mesh from stream.
		/// </summary>
		/// <param name="stream">Stream to read from.</param>
		/// <returns>Complete parsed mesh.</returns>
		public IMesh Load(Stream stream)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
