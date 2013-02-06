using OpenTK;

namespace Toe.Utils.Mesh.Bsp.Q3
{
	public struct Quake3Face
	{
		public int texinfo_id; // The index into the texture array 

		public int effect; // The index for the effects (or -1 = n/a) 

		public int type; // 1=polygon, 2=patch, 3=mesh, 4=billboard 

		public int vertexIndex; // The index into this face's first vertex 

		public int numOfVerts; // The number of vertices for this face 

		public int meshVertIndex; // The index into the first meshvertex 

		public int numMeshVerts; // The number of mesh vertices 

		public int lightmapID; // The texture index for the lightmap 

		public int lMapCornerX; // The face's lightmap corner in the image 

		public int lMapCornerY; // The face's lightmap corner in the image 

		public int lMapSizeX; // The size of the lightmap section 

		public int lMapSizeY; // The size of the lightmap section 

		public Vector3 lMapPos; // The 3D origin of lightmap. 

		public Vector3 lMapBitsetsS; // The 3D space for s and t unit vectors. 

		public Vector3 lMapBitsetsT; // The 3D space for s and t unit vectors. 

		public Vector3 vNormal; // The face normal. 

		public int sizeX; // The bezier patch dimensions. 

		public int sizeY; // The bezier patch dimensions. 
	}
}