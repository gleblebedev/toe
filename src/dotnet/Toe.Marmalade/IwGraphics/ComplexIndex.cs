namespace Toe.Marmalade.IwGraphics
{
	public struct ComplexIndex
	{
		public int Vertex;

		public int Normal;

		public int UV0;

		public int UV1;

		public int Color;

		public int Binormal;

		public int Tangent;

		public int GetUV(int channel)
		{
			switch (channel)
			{
				case 0:
					return UV0;
				case 1:
					return UV0;
				default:
					throw new System.ArgumentOutOfRangeException();
			}
		}
	}
}