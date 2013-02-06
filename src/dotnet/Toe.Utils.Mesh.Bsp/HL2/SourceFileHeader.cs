namespace Toe.Utils.Mesh.Bsp.HL2
{
	public struct SourceFileHeader
	{
		public uint magic; // magic number ("VBSP")

		public uint version;

		public SourceFileEntry Entities; //Map entities

		public SourceFileEntry Planes; //Plane array

		public SourceFileEntry Texdata; //Index to texture names

		public SourceFileEntry Vertexes; //Vertex array

		public SourceFileEntry Visibility; //Compressed visibility bit arrays

		public SourceFileEntry Nodes; //BSP tree nodes  

		public SourceFileEntry Texinfo; //Face texture array

		public SourceFileEntry Faces; //Face array

		public SourceFileEntry Lighting; //Lightmap samples

		public SourceFileEntry Occlusion; //Occlusion data(?)

		public SourceFileEntry Leafs; //BSP tree leaf nodes

		public SourceFileEntry Unused11; //

		public SourceFileEntry Edges; //Edge array

		public SourceFileEntry Surfedges; //Index of edges

		public SourceFileEntry Models; //Brush models (geometry of brush entities)

		public SourceFileEntry Worldlights; //Light entities

		public SourceFileEntry LeafFaces; //Index to faces in each leaf

		public SourceFileEntry LeafBrushes; //Index to brushes in each leaf

		public SourceFileEntry Brushes; //Brush array

		public SourceFileEntry Brushsides; //Brushside array

		public SourceFileEntry Areas; //Area array

		public SourceFileEntry AreaPortals; //Portals between areas

		public SourceFileEntry Portals; //Polygons defining the boundary between adjacent leaves(?)

		public SourceFileEntry Clusters; //Leaves that are enterable by the player

		public SourceFileEntry PortalVerts; //Vertices of portal polygons

		public SourceFileEntry Clusterportals; //Polygons defining the boundary between adjacent clusters(?)

		public SourceFileEntry Dispinfo; //Displacement surface array

		public SourceFileEntry OriginalFaces; //Brush faces array before BSP splitting

		public SourceFileEntry Unused28; //

		public SourceFileEntry PhysCollide; //Physics collision data(?)

		public SourceFileEntry VertNormals; //Vertex normals(?)

		public SourceFileEntry VertNormalIndices; //Vertex normal index array(?)

		public SourceFileEntry DispLightmapAlphas; //Displacement lightmap data(?)

		public SourceFileEntry DispVerts; //Vertices of displacement surface meshes

		public SourceFileEntry DispLightmapSamplePos; //Displacement lightmap data(?)

		public SourceFileEntry GameLump; //Game-specific data lump

		public SourceFileEntry LeafWaterData; // (?)

		public SourceFileEntry Primitives; //Non-polygonal primatives(?)

		public SourceFileEntry PrimVerts; // (?)

		public SourceFileEntry PrimIndices; //(?)

		public SourceFileEntry Pakfile; //Embedded uncompressed-Zip format file

		public SourceFileEntry ClipPortalVerts; //(?)

		public SourceFileEntry Cubemaps; //Env_cubemap location array

		public SourceFileEntry TexdataStringData; //Texture name data

		public SourceFileEntry TexdataStringTable; //Index array into texdata string data

		public SourceFileEntry Overlays; //Info_overlay array       

		public SourceFileEntry LeafMinDistToWater; //(?)

		public SourceFileEntry FaceMacroTextureInfo; //(?)

		public SourceFileEntry DispTris; //Displacement surface triangles

		public SourceFileEntry PhysCollideSurface; //Physics collision surface data(?)

		public SourceFileEntry Unused50; //

		public SourceFileEntry Unused51; //

		public SourceFileEntry Unused52; //

		public SourceFileEntry LightingHDR; //HDR related lighting data(?)

		public SourceFileEntry WorldlightsHDR; //HDR related worldlight data(?)

		public SourceFileEntry LeaflightHDR1; //HDR related leaf lighting data(?)

		public SourceFileEntry LeaflightHDR2; //HDR related leaf lighting data(?)

		public SourceFileEntry Unused57;

		public SourceFileEntry Unused58;

		public SourceFileEntry Unused59;

		public SourceFileEntry Unused60;

		public SourceFileEntry Unused61;

		public SourceFileEntry Unused62;

		public SourceFileEntry Unused63;

		public uint revision;
	}
}