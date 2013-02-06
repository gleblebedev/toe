using System.IO;

namespace Toe.Utils.Mesh.Bsp.Q3
{
	public class BspReader:BaseBspReader, IBspReader
	{
		protected Quake3FileHeader header;

		protected Quake3Vertex[] vertices;
		protected Quake3Face[] faces;

		protected override void ReadHeader()
		{
			header.magic = Stream.ReadUInt32();
			header.version = Stream.ReadUInt32();

			ReadEntry(ref header.entities);
			ReadEntry(ref header.textures);
			ReadEntry(ref header.planes); //Planes used by map geometry.
			ReadEntry(ref header.nodes); //BSP tree nodes.
			ReadEntry(ref header.leafs); //BSP tree leaves.
			ReadEntry(ref header.leaffaces); //Lists of face indices, one list per leaf.
			ReadEntry(ref header.leafbrushes); //Lists of brush indices, one list per leaf.
			ReadEntry(ref header.models); //Descriptions of rigid world geometry in map.
			ReadEntry(ref header.brushes); //Convex polyhedra used to describe solid space.
			ReadEntry(ref header.brushsides); //Brush surfaces.
			ReadEntry(ref header.vertexes); //Vertices used to describe faces.
			ReadEntry(ref header.meshverts); //Lists of offsets, one list per mesh.
			ReadEntry(ref header.effects); //List of special map effects.
			ReadEntry(ref header.faces); //Surface geometry.
			ReadEntry(ref header.lightmaps); //Packed lightmap data.
			ReadEntry(ref header.lightvols); //Local illumination data.
			ReadEntry(ref header.visdata); //Cluster-cluster visibility data.
		}
		protected override void ReadFaces()
		{
			SeekEntryAt(header.faces.offset);
			int size = EvalNumItems(header.faces.size, 26 * 4);
			faces = new Quake3Face[size];
			for (int i = 0; i < size; ++i)
			{
				this.ReadFace(ref this.faces[i]);
			}
			AssertStreamPossition(header.faces.size + header.faces.offset);
		}

		protected virtual void ReadFace(ref Quake3Face quake3Face)
		{
			quake3Face.texinfo_id = Stream.ReadInt32();
			quake3Face.effect = Stream.ReadInt32();
			quake3Face.type = Stream.ReadInt32();
			quake3Face.vertexIndex = Stream.ReadInt32();
			quake3Face.numOfVerts = Stream.ReadInt32();
			quake3Face.meshVertIndex = Stream.ReadInt32();
			quake3Face.numMeshVerts = Stream.ReadInt32();
			quake3Face.lightmapID = Stream.ReadInt32();
			quake3Face.lMapCornerX = Stream.ReadInt32();
			quake3Face.lMapCornerY = Stream.ReadInt32();
			quake3Face.lMapSizeX = Stream.ReadInt32();
			quake3Face.lMapSizeY = Stream.ReadInt32();
			quake3Face.lMapPos.X = Stream.ReadSingle();
			quake3Face.lMapPos.Y = Stream.ReadSingle();
			quake3Face.lMapPos.Z = Stream.ReadSingle();
			quake3Face.lMapBitsetsS.X = Stream.ReadSingle();
			quake3Face.lMapBitsetsS.Y = Stream.ReadSingle();
			quake3Face.lMapBitsetsS.Z = Stream.ReadSingle();
			quake3Face.lMapBitsetsT.X = Stream.ReadSingle();
			quake3Face.lMapBitsetsT.Y = Stream.ReadSingle();
			quake3Face.lMapBitsetsT.Z = Stream.ReadSingle();
			quake3Face.vNormal.X = Stream.ReadSingle();
			quake3Face.vNormal.Y = Stream.ReadSingle();
			quake3Face.vNormal.Z = Stream.ReadSingle();
			quake3Face.sizeX = Stream.ReadInt32();
			quake3Face.sizeY = Stream.ReadInt32();
		}

		protected virtual void ReadEntry(ref Quake3FileEntry entities)
		{
			entities.offset = Stream.ReadUInt32();
			entities.size = Stream.ReadUInt32();
		}

		protected override void ReadVertices()
		{
			SeekEntryAt(header.vertexes.offset);
			int size = EvalNumItems(header.vertexes.size, 44);
			vertices = new Quake3Vertex[size];
			for (int i = 0; i < size; ++i)
			{
				this.ReadVertex(ref this.vertices[i]);
			}
			AssertStreamPossition(header.vertexes.size + header.vertexes.offset);

		}

		protected void ReadVertex(ref Quake3Vertex vertex)
		{
			this.Stream.ReadVector3(out vertex.vPosition);
			this.Stream.ReadVector2(out vertex.vTextureCoord);
			this.Stream.ReadVector2(out vertex.vLightmapCoord);
			if (vertex.vLightmapCoord.X < 0)
			{
				vertex.vLightmapCoord.X = 0;
			}
			if (vertex.vLightmapCoord.X > 1)
			{
				vertex.vLightmapCoord.X = 1;
			}
			if (vertex.vLightmapCoord.Y < 0)
			{
				vertex.vLightmapCoord.Y = 0;
			}
			if (vertex.vLightmapCoord.Y > 1)
			{
				vertex.vLightmapCoord.Y = 1;
			}
			this.Stream.ReadVector3(out vertex.vNormal);

			var lengthSquared = vertex.vNormal.LengthSquared;
			if (lengthSquared < 0.9f || lengthSquared > 1.1f)
			{
				throw new BspFormatException("Probably wrong format of vertex");
			}

			vertex.color = this.Stream.ReadARGB();
		}
	}
}
