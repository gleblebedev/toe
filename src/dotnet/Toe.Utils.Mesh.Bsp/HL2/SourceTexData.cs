using Toe.Utils.ToeMath;

namespace Toe.Utils.Mesh.Bsp.HL2
{
	public struct SourceTexData
	{
		#region Constants and Fields

		public int height; // source image

		public string name; //calculated

		public int nameStringTableID; // index into TexdataStringTable

		public Float3 reflectivity; // RGB reflectivity 	

		public int view_height;

		public int view_width;

		public int width; // source image

		#endregion
	};
}