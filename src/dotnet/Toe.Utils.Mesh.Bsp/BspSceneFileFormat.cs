using System;

namespace Toe.Utils.Mesh.Bsp
{
	public class BspSceneFileFormat : ISceneFileFormat
	{
		#region Implementation of ISceneFileFormat

		public ISceneReader CreateReader()
		{
			return new BspReader();
		}

		public bool CanLoad(string filename)
		{
			if (filename.EndsWith(".bsp", StringComparison.InvariantCultureIgnoreCase)) return true;
			return false;
		}

		#endregion
	}
}