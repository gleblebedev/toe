using System;
using System.Collections.Generic;
using System.Linq;

namespace Toe.Utils.Mesh.Svg
{
	public class SvgSceneFileFormat : ISceneFileFormat
	{
		#region Public Methods and Operators

		/// <summary>
		/// Scene file format name.
		/// </summary>
		public string Name
		{
			get
			{
				return "Scalable Vector Graphics";
			}
		}

		/// <summary>
		/// Scene file format extensions.
		/// </summary>
		string[] extensions = new[] { ".svg" };

		/// <summary>
		/// Scene file format extensions.
		/// </summary>
		public IEnumerable<string> Extensions
		{
			get
			{
				return this.extensions;
			}
		}

		public bool CanLoad(string filename)
		{
			return (from extension in this.extensions where filename.EndsWith(extension, StringComparison.InvariantCultureIgnoreCase) select extension).Any();
		}

		public ISceneReader CreateReader()
		{
			return new SvgReader();
		}

		#endregion
	}
}