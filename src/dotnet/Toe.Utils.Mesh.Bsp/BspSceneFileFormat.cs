using System;

namespace Toe.Utils.Mesh.Bsp
{
	public class BspSceneFileFormat : ISceneFileFormat
	{
		#region Public Methods and Operators

		public bool CanLoad(string filename)
		{
			if (filename.EndsWith(".bsp", StringComparison.InvariantCultureIgnoreCase))
			{
				return true;
			}
			return false;
		}

		public ISceneReader CreateReader()
		{
			return new BspReader();
		}

		#endregion
	}
}