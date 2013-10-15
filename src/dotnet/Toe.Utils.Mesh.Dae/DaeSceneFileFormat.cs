using System;
using System.Collections.Generic;
using System.Linq;

namespace Toe.Utils.Mesh.Dae
{
	public class DaeSceneFileFormat : ISceneFileFormat
	{
		private readonly IStreamConverterFactory streamConverterFactory;

		public DaeSceneFileFormat(IStreamConverterFactory streamConverterFactory)
		{
			this.streamConverterFactory = streamConverterFactory;
		}

		#region Constants and Fields

		/// <summary>
		/// Scene file format extensions.
		/// </summary>
		private readonly string[] extensions = new[] { ".dae" };

		#endregion

		#region Public Properties

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

		#endregion

		#region Public Methods and Operators

		public bool CanLoad(string filename)
		{
			return (from extension in this.extensions
			        where filename.EndsWith(extension, StringComparison.InvariantCultureIgnoreCase)
			        select extension).Any();
		}

		public ISceneReader CreateReader()
		{
			return new DaeReader(streamConverterFactory);
		}

		#endregion
	}
}