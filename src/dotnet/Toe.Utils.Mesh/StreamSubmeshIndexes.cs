using System;

namespace Toe.Utils.Mesh
{
	public struct StreamSubmeshIndexes
	{
		#region Constants and Fields

		public int Binormal;

		public int Color;

		public int Normal;

		public int Tangent;

		public int UV0;

		public int UV1;

		public int Vertex;

		#endregion

		#region Public Methods and Operators

		public int GetUV(int channel)
		{
			switch (channel)
			{
				case 0:
					return this.UV0;
				case 1:
					return this.UV1;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		#endregion
	}
}