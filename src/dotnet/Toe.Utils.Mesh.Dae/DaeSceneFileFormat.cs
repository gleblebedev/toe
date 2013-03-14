using System;
using System.Collections.Generic;
using System.Linq;

namespace Toe.Utils.Mesh.Dae
{
	public class DaeSceneFileFormat : ISceneFileFormat
	{
		#region Public Methods and Operators

		/// <summary>
		/// Scene file format name.
		/// </summary>
		public string Name
		{
			get
			{
				return "Collada DAE";
			}
		}

		/// <summary>
		/// Scene file format extensions.
		/// </summary>
		string[] extensions = new[] { ".dae" };

		/// <summary>
		/// Scene file format extensions.
		/// </summary>
		public IEnumerable<string> Extensions
		{
			get
			{
				return extensions;
			}
		}

		public bool CanLoad(string filename)
		{
			return (from extension in extensions where filename.EndsWith(extension, StringComparison.InvariantCultureIgnoreCase) select extension).Any();
		}

		public ISceneReader CreateReader()
		{
			return new DaeReader();
		}

		#endregion
	}
}