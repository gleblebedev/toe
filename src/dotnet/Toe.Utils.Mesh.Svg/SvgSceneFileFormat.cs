using System;
using System.Collections.Generic;
using System.Linq;

using Autofac;

namespace Toe.Utils.Mesh.Svg
{
	public class SvgSceneFileFormat : ISceneFileFormat
	{
		#region Constants and Fields

		private readonly IComponentContext context;

		/// <summary>
		/// Scene file format extensions.
		/// </summary>
		private readonly string[] extensions = new[] { ".svg" };

		#endregion

		#region Constructors and Destructors

		public SvgSceneFileFormat(IComponentContext context)
		{
			this.context = context;
		}

		public SvgSceneFileFormat()
		{
		}

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
				return "Scalable Vector Graphics";
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
			if (this.context == null)
			{
				return new SvgReader(new SvgReaderOptions());
			}
			return this.context.Resolve<SvgReader>();
		}

		#endregion
	}
}