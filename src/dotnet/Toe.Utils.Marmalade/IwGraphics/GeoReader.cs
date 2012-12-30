using System;
using System.IO;

using Toe.Resources;
#if WINDOWS_PHONE
using Microsoft.Xna.Framework;
#else

#endif

namespace Toe.Utils.Mesh.Marmalade.IwGraphics
{
	/// <summary>
	/// Marmalade SDK .geo file parser.
	/// </summary>
	public class GeoReader : IMeshReader
	{
		#region Constants and Fields

		private readonly IResourceManager resourceManager;

		#endregion

		#region Constructors and Destructors

		public GeoReader(IResourceManager resourceManager)
		{
			this.resourceManager = resourceManager;
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Load mesh from stream.
		/// </summary>
		/// <param name="stream">Stream to read from.</param>
		/// <returns>Complete parsed mesh.</returns>
		public IMesh Load(Stream stream)
		{
			//var m = new TextResourceFormat(resourceManager);
			//var model = m.Load(stream, Directory.GetCurrentDirectory());
			//if (model.Count == 0) return null;
			//var model1 = ((Model)model[0]);
			//if (model1.Meshes.Count == 0) return null;
			//if (model1.Meshes.Count == 1) return model1.Meshes[0];

			throw new NotImplementedException();
		}

		#endregion
	}
}