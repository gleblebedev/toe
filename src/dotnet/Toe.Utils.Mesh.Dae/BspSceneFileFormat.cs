using System;

namespace Toe.Utils.Mesh.Dae
{
	public class DaeSceneFileFormat : ISceneFileFormat
	{
		#region Implementation of ISceneFileFormat

		public ISceneReader CreateReader()
		{
			return new DaeReader();
		}

		public bool CanLoad(string filename)
		{
			if (filename.EndsWith(".dae", StringComparison.InvariantCultureIgnoreCase)) return true;
			return false;
		}

		#endregion
	}
}