using System;

namespace Toe.Utils.Mesh.Bsp.Q2
{
	public class BspReader : BaseBspReader, IBspReader
	{
		#region Methods

		public BspReader(IStreamConverterFactory streamConverterFactory)
			: base(streamConverterFactory)
		{
		}

		protected override void CreateMesh()
		{
			throw new NotImplementedException();
		}

		protected override void ReadHeader()
		{
			throw new NotImplementedException();
		}

		protected override void ReadVertices()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}