using OpenTK;

namespace Toe.Utils.Mesh.Bsp.HL2
{
	public struct SourceTexData
	{
		public Vector3 reflectivity;           // RGB reflectivity 	
		public int nameStringTableID;      // index into TexdataStringTable
		public int width, height;         	// source image
		public int view_width, view_height;

		public string name; //calculated

	};
}