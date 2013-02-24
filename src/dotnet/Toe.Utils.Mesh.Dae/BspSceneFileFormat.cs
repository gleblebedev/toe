using System;

namespace Toe.Utils.Mesh.Dae
{
	public class DaeSceneFileFormat : ISceneFileFormat
	{
		#region Public Methods and Operators

		public bool CanLoad(string filename)
		{
			if (filename.EndsWith(".dae", StringComparison.InvariantCultureIgnoreCase))
			{
				return true;
			}
			return false;
		}

		public ISceneReader CreateReader()
		{
			return new DaeReader();
		}

		#endregion
	}
}