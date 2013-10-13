using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

using OpenTK;

namespace Toe.Utils.Mesh.Bsp.Q3
{
	public class BspReader : BaseBspReader, IBspReader
	{
		#region Constants and Fields

		protected Quake3Face[] faces;

		protected Quake3FileHeader header;

		private Quake3Effects[] effects;

		private uint[] meshverts;

		private Quake3Texture[] textures;

		private SeparateStreamsMesh streamMesh;

		

		#endregion

		#region Methods

		protected override void CreateMesh()
		{
			this.streamMesh = new SeparateStreamsMesh();
			this.meshStreams = new BspMeshStreams();
			meshStreams.Positions = streamMesh.SetStream(Streams.Position, 0, new ListMeshStream<Vector3>());
			meshStreams.Normals = streamMesh.SetStream(Streams.Normal, 0, new ListMeshStream<Vector3>());
			meshStreams.Colors = streamMesh.SetStream(Streams.Color, 0, new ListMeshStream<Color>());
			meshStreams.TexCoord0 = streamMesh.SetStream(Streams.TexCoord, 0, new ListMeshStream<Vector2>());
			meshStreams.TexCoord1 = streamMesh.SetStream(Streams.TexCoord, 1, new ListMeshStream<Vector2>());
		}

		protected override void BuildScene()
		{
			var maxTextures = this.textures.Length;

			SeparateStreamsSubmesh[] submeshes = new SeparateStreamsSubmesh[maxTextures];
			this.BuildSubmeshes(maxTextures, submeshes, streamMesh, meshStreams);
			var submeshStreams = submeshes.Select(x => (x == null)?null: new BspSubmeshStreams(x, meshStreams)).ToArray();

			var node = new Node();
			this.Scene.Nodes.Add(node);
			node.Mesh = streamMesh;

			foreach (var quake3Face in this.faces)
			{
				switch (quake3Face.type)
				{
					case 2: //Patch
						this.BuildPatch(quake3Face, submeshStreams[quake3Face.texinfo_id]);

						break;
					case 1: //Polygon
					case 3: //Mesh
						for (int i = 0; i < quake3Face.numMeshVerts; ++i)
						{

							AddVertexToMesh((int)(quake3Face.vertexIndex + this.meshverts[quake3Face.meshVertIndex + i]),submeshStreams[quake3Face.texinfo_id]);
						}
						break;
					case 4: //Billboard
						break;
				}
			}
			this.Scene.Geometries.Add(streamMesh);
		}

		protected override void ReadEffects()
		{
			this.SeekEntryAt(this.header.effects.offset);
			int size = this.EvalNumItems(this.header.effects.size, 64 + 4 + 4);
			this.effects = new Quake3Effects[size];
			for (int i = 0; i < size; ++i)
			{
				this.ReadEffect(ref this.effects[i]);
			}
			this.AssertStreamPossition(this.header.effects.size + this.header.effects.offset);
		}

		protected virtual void ReadEntry(ref Quake3FileEntry entities)
		{
			entities.offset = this.Stream.ReadUInt32();
			entities.size = this.Stream.ReadUInt32();
		}

		protected virtual void ReadFace(ref Quake3Face quake3Face)
		{
			quake3Face.texinfo_id = this.Stream.ReadInt32();
			quake3Face.effect = this.Stream.ReadInt32();
			quake3Face.type = this.Stream.ReadInt32();
			if (quake3Face.type > 4)
			{
				throw new BspFormatException("Unknown face type " + quake3Face.type);
			}
			quake3Face.vertexIndex = this.Stream.ReadInt32();
			quake3Face.numOfVerts = this.Stream.ReadInt32();
			quake3Face.meshVertIndex = this.Stream.ReadInt32();
			quake3Face.numMeshVerts = this.Stream.ReadInt32();
			quake3Face.lightmapID = this.Stream.ReadInt32();
			quake3Face.lMapCornerX = this.Stream.ReadInt32();
			quake3Face.lMapCornerY = this.Stream.ReadInt32();
			quake3Face.lMapSizeX = this.Stream.ReadInt32();
			quake3Face.lMapSizeY = this.Stream.ReadInt32();
			quake3Face.lMapPos.X = this.Stream.ReadSingle();
			quake3Face.lMapPos.Y = this.Stream.ReadSingle();
			quake3Face.lMapPos.Z = this.Stream.ReadSingle();
			quake3Face.lMapBitsetsS.X = this.Stream.ReadSingle();
			quake3Face.lMapBitsetsS.Y = this.Stream.ReadSingle();
			quake3Face.lMapBitsetsS.Z = this.Stream.ReadSingle();
			quake3Face.lMapBitsetsT.X = this.Stream.ReadSingle();
			quake3Face.lMapBitsetsT.Y = this.Stream.ReadSingle();
			quake3Face.lMapBitsetsT.Z = this.Stream.ReadSingle();
			quake3Face.vNormal.X = this.Stream.ReadSingle();
			quake3Face.vNormal.Y = this.Stream.ReadSingle();
			quake3Face.vNormal.Z = this.Stream.ReadSingle();
			quake3Face.sizeX = this.Stream.ReadInt32();
			quake3Face.sizeY = this.Stream.ReadInt32();
		}

		protected override void ReadFaces()
		{
			this.ReadMeshVerts();

			this.SeekEntryAt(this.header.faces.offset);
			int size = this.EvalNumItems(this.header.faces.size, 26 * 4);
			this.faces = new Quake3Face[size];
			for (int i = 0; i < size; ++i)
			{
				this.ReadFace(ref this.faces[i]);
			}
			this.AssertStreamPossition(this.header.faces.size + this.header.faces.offset);
		}

		protected override void ReadHeader()
		{
			this.header.magic = this.Stream.ReadUInt32();
			this.header.version = this.Stream.ReadUInt32();

			this.ReadEntry(ref this.header.entities);
			this.ReadEntry(ref this.header.textures);
			this.ReadEntry(ref this.header.planes); //Planes used by map geometry.
			this.ReadEntry(ref this.header.nodes); //BSP tree nodes.
			this.ReadEntry(ref this.header.leafs); //BSP tree leaves.
			this.ReadEntry(ref this.header.leaffaces); //Lists of face indices, one list per leaf.
			this.ReadEntry(ref this.header.leafbrushes); //Lists of brush indices, one list per leaf.
			this.ReadEntry(ref this.header.models); //Descriptions of rigid world geometry in map.
			this.ReadEntry(ref this.header.brushes); //Convex polyhedra used to describe solid space.
			this.ReadEntry(ref this.header.brushsides); //Brush surfaces.
			this.ReadEntry(ref this.header.vertexes); //Vertices used to describe faces.
			this.ReadEntry(ref this.header.meshverts); //Lists of offsets, one list per mesh.
			this.ReadEntry(ref this.header.effects); //List of special map effects.
			this.ReadEntry(ref this.header.faces); //Surface geometry.
			this.ReadEntry(ref this.header.lightmaps); //Packed lightmap data.
			this.ReadEntry(ref this.header.lightvols); //Local illumination data.
			this.ReadEntry(ref this.header.visdata); //Cluster-cluster visibility data.
		}

		protected override void ReadTextures()
		{
			this.SeekEntryAt(this.header.textures.offset);
			int size = this.EvalNumItems(this.header.textures.size, 64 + 4 + 4);
			this.textures = new Quake3Texture[size];
			for (int i = 0; i < size; ++i)
			{
				this.ReadTexture(ref this.textures[i]);
			}
			this.AssertStreamPossition(this.header.textures.size + this.header.textures.offset);
		}

		protected void ReadVertex()
		{
			meshStreams.Positions.Add(this.Stream.ReadVector3());
			meshStreams.TexCoord0.Add(this.Stream.ReadVector2());
			var vLightmapCoord = this.Stream.ReadVector2();
			if (vLightmapCoord.X < 0)
			{
				vLightmapCoord.X = 0;
			}
			if (vLightmapCoord.X > 1)
			{
				vLightmapCoord.X = 1;
			}
			if (vLightmapCoord.Y < 0)
			{
				vLightmapCoord.Y = 0;
			}
			if (vLightmapCoord.Y > 1)
			{
				vLightmapCoord.Y = 1;
			}
			meshStreams.TexCoord1.Add(vLightmapCoord);
			var vNormal = this.Stream.ReadVector3();
			meshStreams.Normals.Add(vNormal);

			var lengthSquared = vNormal.LengthSquared;
			if (lengthSquared != 0.0f && (lengthSquared < 0.9f || lengthSquared > 1.1f))
			{
				throw new BspFormatException("Probably wrong format of vertex");
			}
			meshStreams.Colors.Add(this.Stream.ReadBGRA());
		}

		protected override void ReadVertices()
		{
			this.SeekEntryAt(this.header.vertexes.offset);
			int size = this.EvalNumItems(this.header.vertexes.size, 44);
			for (int i = 0; i < size; ++i)
			{
				this.ReadVertex();
			}
			this.AssertStreamPossition(this.header.vertexes.size + this.header.vertexes.offset);
		}

		private static void AddVertexToMesh(int index, BspSubmeshStreams subMesh)
		{
			subMesh.AddToAllStreams(index);
		}

		private void BuildPatch(Quake3Face quake3Face, BspSubmeshStreams submesh)
		{
			var width = quake3Face.sizeX;
			var height = quake3Face.sizeY;
			if (width * height != quake3Face.numOfVerts)
			{
				throw new BspFormatException("wrong patch point count");
			}
			for (int i = 0; i < width - 1; i += 1)
			{
				for (int j = 0; j < height - 1; j += 1)
				{
					AddVertexToMesh(quake3Face.vertexIndex + (i) + (j) * width, submesh);
					AddVertexToMesh(quake3Face.vertexIndex + (i + 1) + (j + 1) * width, submesh);
					AddVertexToMesh(quake3Face.vertexIndex + (i + 1) + (j) * width, submesh);

					AddVertexToMesh(quake3Face.vertexIndex + (i) + (j) * width, submesh);
					AddVertexToMesh(quake3Face.vertexIndex + (i) + (j + 1) * width, submesh);
					AddVertexToMesh(quake3Face.vertexIndex + (i + 1) + (j + 1) * width, submesh);
				}
			}
		}

		private void BuildSubmeshes(int maxTextures, SeparateStreamsSubmesh[] submeshes, SeparateStreamsMesh streamMesh, BspMeshStreams meshStreams)
		{
			int[] textureToMaterial = new int[maxTextures];
			foreach (var quake3Face in this.faces)
			{
				++textureToMaterial[quake3Face.texinfo_id];
			}
			for (int i = 0; i < maxTextures; ++i)
			{
				if (textureToMaterial[i] > 0)
				{
					submeshes[i] = streamMesh.CreateSubmesh();
					int index = this.Scene.Materials.Count;
					var baseFileName = Path.Combine(this.GameRootPath, this.textures[i].name);
					var imagePath = baseFileName;
					if (!File.Exists(imagePath))
					{
						imagePath = baseFileName + ".jpg";
						if (!File.Exists(imagePath))
						{
							imagePath = baseFileName + ".png";
							if (!File.Exists(imagePath))
							{
								imagePath = baseFileName + ".tga";
								if (!File.Exists(imagePath))
								{
									imagePath = this.textures[i].name;
								}
							}
						}
					}
					var texture = new FileReferenceImage { Path = imagePath };
					this.Scene.Images.Add(texture);
					var effect = new SceneEffect { Diffuse = new ImageColorSource { Image = texture }, CullMode = CullMode.Front};
					this.Scene.Effects.Add(effect);
					var sceneMaterial = new SceneMaterial { Effect = effect };
					this.Scene.Materials.Add(sceneMaterial);
					submeshes[i].Material = sceneMaterial;
					textureToMaterial[i] = index;
				}
			}
		}

		private void ReadEffect(ref Quake3Effects quake3Effects)
		{
			var nameBytes = this.Stream.ReadBytes(64);
			var nameRaw = Encoding.ASCII.GetString(nameBytes);
			quake3Effects.name = nameRaw.Trim(new[] { ' ', '\0' });
			quake3Effects.brush = this.Stream.ReadUInt32();
			quake3Effects.unknown = this.Stream.ReadUInt32();
		}

		private void ReadMeshVerts()
		{
			this.SeekEntryAt(this.header.meshverts.offset);
			int size = this.EvalNumItems(this.header.meshverts.size, 4);
			this.meshverts = new uint[size];
			for (int i = 0; i < size; ++i)
			{
				this.meshverts[i] = this.Stream.ReadUInt32();
			}
			this.AssertStreamPossition(this.header.meshverts.size + this.header.meshverts.offset);
		}

		private void ReadTexture(ref Quake3Texture quake3Texture)
		{
			var nameBytes = this.Stream.ReadBytes(64);
			var nameRaw = Encoding.ASCII.GetString(nameBytes);
			var trimmedName = nameRaw.TrimEnd(new[] { ' ', '\0' });
			//Removing "2" at the end of the name
			//if (trimmedName.EndsWith("2"))
			//    trimmedName = trimmedName.Substring(0, trimmedName.Length - 1);
			//else
			//{
			//    trimmedName = trimmedName;
			//}

			quake3Texture.name = trimmedName;
			quake3Texture.flags = this.Stream.ReadUInt32();
			quake3Texture.contents = this.Stream.ReadUInt32();
		}

		#endregion
	}

	public struct Quake3Texture
	{
		#region Constants and Fields

		public uint contents;

		public uint flags;

		public string name;

		#endregion
	}

	public struct Quake3Effects
	{
		#region Constants and Fields

		public uint brush;

		public string name;

		public uint unknown;

		#endregion
	}
}