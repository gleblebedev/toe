using System.Collections.Generic;
using System.IO;

using OpenTK;

namespace Toe.Utils.Mesh.Bsp.HL2
{
	public class BaseHL2BspReader : BaseBspReader
	{

		protected SourceFileHeader header = new SourceFileHeader();

		private Vector3[] vertices;

		protected virtual void ReadEntry(ref SourceFileEntry entry)
		{
			entry.offset = Stream.ReadUInt32();
			entry.size = Stream.ReadUInt32();
			entry.version = Stream.ReadUInt32();
			entry.magic = Stream.ReadUInt32();
		}
		
		protected override void ReadVertices()
		{
			SeekEntryAt(header.Vertexes.offset);
			int size = EvalNumItems(header.Vertexes.size, 12);
			vertices = new Vector3[size];
			for (int i = 0; i < size; ++i)
			{
				Stream.ReadVector3(out vertices[i]);
			}
			AssertStreamPossition(header.Vertexes.size + header.Vertexes.offset);
		}

	

		protected override void ReadHeader()
		{
			header.magic = Stream.ReadUInt32();
			header.version = Stream.ReadUInt32();

			this.ReadEntry(ref header.Entities);
			this.ReadEntry(ref header.Planes); //Plane array
			this.ReadEntry(ref header.Texdata); //Index to texture names
			this.ReadEntry(ref header.Vertexes); //Vertex array
			this.ReadEntry(ref header.Visibility); //Compressed visibility bit arrays
			this.ReadEntry(ref header.Nodes); //BSP tree nodes  
			this.ReadEntry(ref header.Texinfo); //Face texture array
			this.ReadEntry(ref header.Faces); //Face array
			this.ReadEntry(ref header.Lighting); //Lightmap samples
			this.ReadEntry(ref header.Occlusion); //Occlusion data(?)
			this.ReadEntry(ref header.Leafs); //BSP tree leaf nodes
			this.ReadEntry(ref header.Unused11); //
			this.ReadEntry(ref header.Edges); //Edge array
			this.ReadEntry(ref header.Surfedges); //Index of edges
			this.ReadEntry(ref header.Models); //Brush models (geometry of brush entities)
			this.ReadEntry(ref header.Worldlights); //Light entities
			this.ReadEntry(ref header.LeafFaces); //Index to faces in each leaf
			this.ReadEntry(ref header.LeafBrushes); //Index to brushes in each leaf
			this.ReadEntry(ref header.Brushes); //Brush array
			this.ReadEntry(ref header.Brushsides); //Brushside array
			this.ReadEntry(ref header.Areas); //Area array
			this.ReadEntry(ref header.AreaPortals); //Portals between areas
			this.ReadEntry(ref header.Portals); //Polygons defining the boundary between adjacent leaves(?)
			this.ReadEntry(ref header.Clusters); //Leaves that are enterable by the player
			this.ReadEntry(ref header.PortalVerts); //Vertices of portal polygons
			this.ReadEntry(ref header.Clusterportals); //Polygons defining the boundary between adjacent clusters(?)
			this.ReadEntry(ref header.Dispinfo); //Displacement surface array
			this.ReadEntry(ref header.OriginalFaces); //Brush faces array before BSP splitting
			this.ReadEntry(ref header.Unused28); //
			this.ReadEntry(ref header.PhysCollide); //Physics collision data(?)
			this.ReadEntry(ref header.VertNormals); //Vertex normals(?)
			this.ReadEntry(ref header.VertNormalIndices); //Vertex normal index array(?)
			this.ReadEntry(ref header.DispLightmapAlphas); //Displacement lightmap data(?)
			this.ReadEntry(ref header.DispVerts); //Vertices of displacement surface meshes
			this.ReadEntry(ref header.DispLightmapSamplePos); //Displacement lightmap data(?)
			this.ReadEntry(ref header.GameLump); //Game-specific data lump
			this.ReadEntry(ref header.LeafWaterData); // (?)
			this.ReadEntry(ref header.Primitives); //Non-polygonal primatives(?)
			this.ReadEntry(ref header.PrimVerts); // (?)
			this.ReadEntry(ref header.PrimIndices); //(?)
			this.ReadEntry(ref header.Pakfile); //Embedded uncompressed-Zip format file
			this.ReadEntry(ref header.ClipPortalVerts); //(?)
			this.ReadEntry(ref header.Cubemaps); //Env_cubemap location array
			this.ReadEntry(ref header.TexdataStringData); //Texture name data
			this.ReadEntry(ref header.TexdataStringTable); //Index array into texdata string data
			this.ReadEntry(ref header.Overlays); //Info_overlay array       
			this.ReadEntry(ref header.LeafMinDistToWater); //(?)
			this.ReadEntry(ref header.FaceMacroTextureInfo); //(?)
			this.ReadEntry(ref header.DispTris); //Displacement surface triangles
			this.ReadEntry(ref header.PhysCollideSurface); //Physics collision surface data(?)
			this.ReadEntry(ref header.Unused50); //
			this.ReadEntry(ref header.Unused51); //
			this.ReadEntry(ref header.Unused52); //
			this.ReadEntry(ref header.LightingHDR); //HDR related lighting data(?)
			this.ReadEntry(ref header.WorldlightsHDR); //HDR related worldlight data(?)
			this.ReadEntry(ref header.LeaflightHDR1); //HDR related leaf lighting data(?)
			this.ReadEntry(ref header.LeaflightHDR2); //HDR related leaf lighting data(?)
			this.ReadEntry(ref header.Unused57);
			this.ReadEntry(ref header.Unused58);
			this.ReadEntry(ref header.Unused59);
			this.ReadEntry(ref header.Unused60);
			this.ReadEntry(ref header.Unused61);
			this.ReadEntry(ref header.Unused62);
			this.ReadEntry(ref header.Unused63);

			header.revision = Stream.ReadUInt32();
		}
	}
}