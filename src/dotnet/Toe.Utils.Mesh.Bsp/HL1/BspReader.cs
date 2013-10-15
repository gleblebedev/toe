using Toe.Utils.Mesh.Bsp.Q1;

namespace Toe.Utils.Mesh.Bsp.HL1
{
	public class BspReader : BaseQ1HL2BspReader, IBspReader
	{
		public BspReader(IStreamConverterFactory streamConverterFactory)
			: base(streamConverterFactory)
		{
		}
	}
}