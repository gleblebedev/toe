using System;

namespace Toe.Utils.Mesh.Ase
{
	public class AseSceneFileFormat: ISceneFileFormat
	{
		#region Implementation of ISceneFileFormat

		public ISceneReader CreateReader()
		{
			return new AseReader();
		}

		public bool CanLoad(string filename)
		{
			if (filename.EndsWith(".ase", StringComparison.InvariantCultureIgnoreCase)) return true;
			return false;
		}

		#endregion
	}
}