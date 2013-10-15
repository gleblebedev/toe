namespace Toe.Utils.Mesh.Bsp.HL2
{
	/// <summary>
	/// BSP file from CS:GO
	/// </summary>
	public class BspReader21 : BspReader20, IBspReader
	{
		//protected override void ReadEntry(ref SourceFileEntry entry)
		//{
		//    entry.version = Stream.ReadUInt32();
		//    entry.offset = Stream.ReadUInt32();
		//    entry.size = Stream.ReadUInt32();
		//    entry.magic = Stream.ReadUInt32();			

		//}
		public BspReader21(IStreamConverterFactory streamConverterFactory)
			: base(streamConverterFactory)
		{
		}
	}
}