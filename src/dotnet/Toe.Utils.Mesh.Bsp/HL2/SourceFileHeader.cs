namespace Toe.Utils.Mesh.Bsp.HL2
{
	public struct SourceFileHeader
	{
		#region Constants and Fields

		public SourceFileEntry AreaPortals; //Portals between areas

		public SourceFileEntry Areas; //Area array

		public SourceFileEntry Brushes; //Brush array

		public SourceFileEntry Brushsides; //Brushside array

		public SourceFileEntry ClipPortalVerts; //(?)

		public SourceFileEntry Clusterportals; //Polygons defining the boundary between adjacent clusters(?)

		public SourceFileEntry Clusters; //Leaves that are enterable by the player

		public SourceFileEntry Cubemaps; //Env_cubemap location array

		public SourceFileEntry DispLightmapAlphas; //Displacement lightmap data(?)

		public SourceFileEntry DispLightmapSamplePos; //Displacement lightmap data(?)

		public SourceFileEntry DispTris; //Displacement surface triangles

		public SourceFileEntry DispVerts; //Vertices of displacement surface meshes

		public SourceFileEntry Dispinfo; //Displacement surface array

		public SourceFileEntry Edges; //Edge array

		public SourceFileEntry Entities; //Map entities

		public SourceFileEntry FaceMacroTextureInfo; //(?)

		public SourceFileEntry Faces; //Face array

		public SourceFileEntry GameLump; //Game-specific data lump

		public SourceFileEntry LeafBrushes; //Index to brushes in each leaf

		public SourceFileEntry LeafFaces; //Index to faces in each leaf

		public SourceFileEntry LeafMinDistToWater; //(?)

		public SourceFileEntry LeafWaterData; // (?)

		public SourceFileEntry LeaflightHDR1; //HDR related leaf lighting data(?)

		public SourceFileEntry LeaflightHDR2; //HDR related leaf lighting data(?)

		public SourceFileEntry Leafs; //BSP tree leaf nodes

		public SourceFileEntry Lighting; //Lightmap samples

		public SourceFileEntry LightingHDR; //HDR related lighting data(?)

		public SourceFileEntry Models; //Brush models (geometry of brush entities)

		public SourceFileEntry Nodes; //BSP tree nodes  

		public SourceFileEntry Occlusion; //Occlusion data(?)

		public SourceFileEntry OriginalFaces; //Brush faces array before BSP splitting

		public SourceFileEntry Overlays; //Info_overlay array       

		public SourceFileEntry Pakfile; //Embedded uncompressed-Zip format file

		public SourceFileEntry PhysCollide; //Physics collision data(?)

		public SourceFileEntry PhysCollideSurface; //Physics collision surface data(?)

		public SourceFileEntry Planes; //Plane array

		public SourceFileEntry PortalVerts; //Vertices of portal polygons

		public SourceFileEntry Portals; //Polygons defining the boundary between adjacent leaves(?)

		public SourceFileEntry PrimIndices; //(?)

		public SourceFileEntry PrimVerts; // (?)

		public SourceFileEntry Primitives; //Non-polygonal primatives(?)

		public SourceFileEntry Surfedges; //Index of edges

		public SourceFileEntry Texdata; //Index to texture names

		public SourceFileEntry TexdataStringData; //Texture name data

		public SourceFileEntry TexdataStringTable; //Index array into texdata string data

		public SourceFileEntry Texinfo; //Face texture array

		public SourceFileEntry Unused11; //

		public SourceFileEntry Unused28; //

		public SourceFileEntry Unused50; //

		public SourceFileEntry Unused51; //

		public SourceFileEntry Unused52; //

		public SourceFileEntry Unused57;

		public SourceFileEntry Unused58;

		public SourceFileEntry Unused59;

		public SourceFileEntry Unused60;

		public SourceFileEntry Unused61;

		public SourceFileEntry Unused62;

		public SourceFileEntry Unused63;

		public SourceFileEntry VertNormalIndices; //Vertex normal index array(?)

		public SourceFileEntry VertNormals; //Vertex normals(?)

		public SourceFileEntry Vertexes; //Vertex array

		public SourceFileEntry Visibility; //Compressed visibility bit arrays

		public SourceFileEntry Worldlights; //Light entities

		public SourceFileEntry WorldlightsHDR; //HDR related worldlight data(?)

		public uint magic; // magic number ("VBSP")

		public uint revision;

		public uint version;

		#endregion
	}
}