using System;

namespace Toe.Utils.Mesh.Ase
{
	public class AseSceneFileFormat : ISceneFileFormat
	{
		#region Public Methods and Operators

		public bool CanLoad(string filename)
		{
			if (filename.EndsWith(".ase", StringComparison.InvariantCultureIgnoreCase))
			{
				return true;
			}
			return false;
		}

		public ISceneReader CreateReader()
		{
			return new AseReader();
		}

		#endregion
	}
}