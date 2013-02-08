using System;
using System.IO;
using System.Text;

using OpenTK;

namespace Toe.Utils.Mesh.Bsp.Q3
{
	public class BspReader:BaseBspReader, IBspReader
	{
		protected Quake3FileHeader header;

		protected Quake3Vertex[] vertices;
		protected Quake3Face[] faces;

		private uint[] meshverts;

		private Quake3Texture[] textures;

		private Quake3Effects[] effects;

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
		protected override void ReadTextures()
		{
			SeekEntryAt(header.textures.offset);
			int size = EvalNumItems(header.textures.size, 64 + 4 + 4);
			textures = new Quake3Texture[size];
			for (int i = 0; i < size; ++i)
			{
				this.ReadTexture(ref this.textures[i]);
			}
			AssertStreamPossition(header.textures.size + header.textures.offset);
		
		}

		private void ReadTexture(ref Quake3Texture quake3Texture)
		{
			quake3Texture.name = Encoding.ASCII.GetString(Stream.ReadBytes(64)).Trim(new char[] { ' ', '\0' });
			quake3Texture.flags = Stream.ReadUInt32();
			quake3Texture.contents = Stream.ReadUInt32();
		}

		protected override void ReadEffects()
		{
			SeekEntryAt(header.effects.offset);
			int size = EvalNumItems(header.effects.size, 64 + 4 + 4);
			effects = new Quake3Effects[size];
			for (int i = 0; i < size; ++i)
			{
				this.ReadEffect(ref this.effects[i]);
			}
			AssertStreamPossition(header.effects.size + header.effects.offset);
		}

		private void ReadEffect(ref Quake3Effects quake3Effects)
		{
			quake3Effects.name = Encoding.ASCII.GetString(Stream.ReadBytes(64)).Trim(new char[] { ' ', '\0' });
			quake3Effects.brush = Stream.ReadUInt32();
			quake3Effects.unknown = Stream.ReadUInt32();
		}

		protected override void ReadFaces()
		{
			this.ReadMeshVerts();

			SeekEntryAt(header.faces.offset);
			int size = EvalNumItems(header.faces.size, 26 * 4);
			faces = new Quake3Face[size];
			for (int i = 0; i < size; ++i)
			{
				this.ReadFace(ref this.faces[i]);
			}
			AssertStreamPossition(header.faces.size + header.faces.offset);
		}

		private void ReadMeshVerts()
		{
			this.SeekEntryAt(this.header.meshverts.offset);
			int size = this.EvalNumItems(this.header.meshverts.size, 4);
			meshverts = new uint[size];
			for (int i = 0; i < size; ++i)
			{
				meshverts[i] = Stream.ReadUInt32();
			}
			this.AssertStreamPossition(this.header.meshverts.size + this.header.meshverts.offset);
		}

		protected override void BuildScene()
		{
			var node = new Node();
			Scene.Nodes.Add(node);
			var streamMesh = new VertexBufferMesh();
			node.Mesh = streamMesh;

			var subMesh = streamMesh.CreateSubmesh() as VertexBufferSubmesh;
			subMesh.VertexSourceType= VertexSourceType.TrianleList;
			foreach (var quake3Face in faces)
			{
				switch (quake3Face.type)
				{
					case 2: //Patch
						BuildPatch(quake3Face, streamMesh, subMesh);
						
						break;
					case 1: //Polygon
					case 3: //Mesh
						for (int i = 0; i < quake3Face.numMeshVerts ; ++i)
						{
							AddVertexToMesh(ref vertices[quake3Face.vertexIndex + meshverts[quake3Face.meshVertIndex + i]],  subMesh, streamMesh);
						}
						break;
					case 4: //Billboard
						break;
				}
			}
			Scene.Geometries.Add(streamMesh);
		}

		private void BuildPatch(Quake3Face quake3Face, VertexBufferMesh streamMesh, VertexBufferSubmesh subMesh)
		{
			var width = quake3Face.sizeX;
			var height = quake3Face.sizeY;
			if (width * height != quake3Face.numOfVerts)
				throw new BspFormatException("wrong patch point count");
			for (int i = 0; i < width - 1; i += 1)
			{
				for (int j = 0; j < height - 1; j += 1)
				{
					AddVertexToMesh(ref vertices[quake3Face.vertexIndex +  (i) + (j) * width], subMesh, streamMesh);
					AddVertexToMesh(ref vertices[quake3Face.vertexIndex +  (i+1) + (j+1) * width], subMesh, streamMesh);
					AddVertexToMesh(ref vertices[quake3Face.vertexIndex + (i + 1) + (j) * width], subMesh, streamMesh);

					AddVertexToMesh(ref vertices[quake3Face.vertexIndex +  (i) + (j) * width], subMesh, streamMesh);
					AddVertexToMesh(ref vertices[quake3Face.vertexIndex +  (i ) + (j + 1) * width], subMesh, streamMesh);
					AddVertexToMesh(ref vertices[quake3Face.vertexIndex + (i + 1) + (j + 1) * width], subMesh, streamMesh);
				}
			}
		}

		private static void AddVertexToMesh(ref Quake3Vertex vertex,  VertexBufferSubmesh subMesh, VertexBufferMesh streamMesh)
		{
			var v = new Vertex()
				{
					Position = vertex.vPosition,
					Normal = vertex.vNormal,
					Color = vertex.color,
					UV0 = new Vector3(vertex.vTextureCoord.X, vertex.vTextureCoord.Y, 0.0f)
				};
			subMesh.Add(streamMesh.VertexBuffer.Add(v));
		}

		protected virtual void ReadFace(ref Quake3Face quake3Face)
		{
			quake3Face.texinfo_id = Stream.ReadInt32();
			quake3Face.effect = Stream.ReadInt32();
			quake3Face.type = Stream.ReadInt32();
			if (quake3Face.type > 4)
				throw new BspFormatException("Unknown face type " + quake3Face.type);
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

	public struct Quake3Texture
	{
		public string name;
		public uint flags;
		public uint contents;
	}
	public struct Quake3Effects
	{
		public string name;
		public uint brush;
		public uint unknown;
	}
}
