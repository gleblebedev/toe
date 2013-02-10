using System.Drawing;
using System.IO;

using OpenTK;

namespace Toe.Utils.Mesh.Bsp.HL2
{
	public class BaseHL2BspReader : BaseBspReader
	{
		#region Constants and Fields

		protected SourceFace[] faces;

		protected SourceFileHeader header;

		protected SourceTexData[] textures;

		private uint[] texdataStringTable;

		private Vector3[] vertices;

		private SourceTexInfo[] texInfo;

		private int[] listOfEdges;

		private SourceEdge[] edges;

		private SourcePlane[] planes;

		#endregion

		#region Methods

		protected override void BuildScene()
		{
			var maxTextures = this.textures.Length;
			var streamMesh = new VertexBufferMesh();
			VertexBufferSubmesh[] submeshes = new VertexBufferSubmesh[maxTextures];

			this.BuildSubmeshes(maxTextures, submeshes, streamMesh);

			var node = new Node();
			this.Scene.Nodes.Add(node);
			node.Mesh = streamMesh;

			for (int index = 0; index < this.faces.Length; index++)
			{
				BuildFace(ref this.faces[index], submeshes[texInfo[this.faces[index].texinfo].texdata], streamMesh);
			}
			this.Scene.Geometries.Add(streamMesh);
		}
		
		private void BuildFace(ref SourceFace face, VertexBufferSubmesh submesh, VertexBufferMesh streamMesh)
		{
			Vertex[] faceVertices = new Vertex[face.numedges];

			var plane = planes[face.planenum];
			var texture_id = (int)texInfo[face.texinfo].texdata;

			Vector2 minUV0 = new Vector2(float.MaxValue, float.MaxValue);
			Vector2 minUV1 = new Vector2(float.MaxValue, float.MaxValue);
			Vector2 maxUV1 = new Vector2(float.MinValue, float.MinValue);
			for (int index = 0; index < faceVertices.Length; index++)
			{
				var listOfEdgesIndex = (int)face.firstedge + index;
				if (listOfEdgesIndex >= listOfEdges.Length)
					throw new BspFormatException(string.Format("Edge list index {0} is out of range [0..{1}]", listOfEdgesIndex, listOfEdges.Length - 1));

				var edgeIndex = listOfEdges[listOfEdgesIndex];
				if (edgeIndex >= edges.Length)
					throw new BspFormatException(string.Format("Edge index {0} is out of range [0..{1}]", edgeIndex, edges.Length - 1));

				SourceEdge edge;
				if (edgeIndex >= 0)
				{
					edge = edges[edgeIndex];
				}
				else
				{
					var flippedEdge = edges[-edgeIndex];
					edge = new SourceEdge() { vertex0 = flippedEdge.vertex1, vertex1 = flippedEdge.vertex0 };
				}
				var edgesvertex0 = edge.vertex0;
				if (edgesvertex0 >= vertices.Length)
					throw new BspFormatException(string.Format("Vertex index {0} is out of range [0..{1}]", edgesvertex0, vertices.Length - 1));
				var edgesvertex1 = edge.vertex1;
				if (edgesvertex1 >= vertices.Length)
					throw new BspFormatException(string.Format("Vertex index {0} is out of range [0..{1}]", edgesvertex1, vertices.Length - 1));

				Vertex vertex;
				BuildVertex(vertices[(short)edgesvertex0], (face.side == 0) ? plane.normal : -plane.normal, face, ref texInfo[face.texinfo], out vertex);
				faceVertices[index] = vertex;
				if (minUV0.X > vertex.UV0.X)
					minUV0.X = vertex.UV0.X;
				if (minUV0.Y > vertex.UV0.Y)
					minUV0.Y = vertex.UV0.Y;
				if (minUV1.X > vertex.UV1.X)
					minUV1.X = vertex.UV1.X;
				if (minUV1.Y > vertex.UV1.Y)
					minUV1.Y = vertex.UV1.Y;
				if (maxUV1.X < vertex.UV1.X)
					maxUV1.X = vertex.UV1.X;
				if (maxUV1.Y < vertex.UV1.Y)
					maxUV1.Y = vertex.UV1.Y;
			}
			if (textures[texture_id].name == "TOOLS/TOOLSSKYBOX")
			{
				minUV0.X = 0;
				minUV0.Y = 0;
				for (int j = 0; j < (int)face.numedges; ++j)
					faceVertices[j].UV0 = new Vector3(0, 0,0);
			}

			int[] indices = new int[faceVertices.Length];
			for (int j = 0; j < faceVertices.Length ; ++j)
			{
				indices[j] = streamMesh.VertexBuffer.Add(faceVertices[j]);
			}
			for (int j = 1; j < faceVertices.Length - 1; ++j)
			{
				submesh.Add(indices[0]);
				submesh.Add(indices[j]);
				submesh.Add(indices[j+1]);
			}

		}
		float safeOffset = 0.5f;//0.5f;
		float safeBorderWidth = 1.0f;
		private void BuildVertex(Vector3 vector3, Vector3 vector4, SourceFace f, ref SourceTexInfo surf, out Vertex res)
		{
			res = new Vertex
				{
					Position = vector3,
					Normal = vector4,
					Color = Color.White,
					UV0 =
						new Vector3(
						Vector3.Dot(surf.vectorS, vector3) + surf.distS, Vector3.Dot(surf.vectorT, vector3) + surf.distT, 0.0f),
					UV1 =
						new Vector3(
						Vector3.Dot(surf.lm_vectorS, vector3) + surf.lm_distS - (float)f.LightmapTextureMinsInLuxels[0],
						Vector3.Dot(surf.lm_vectorT, vector3) + surf.lm_distT - (float)f.LightmapTextureMinsInLuxels[1],
						0.0f)
				};
			//if (f.LightmapTextureSizeInLuxels[0] == 0)
			res.UV1.X = (res.UV1.X + safeOffset) / ((float)f.LightmapTextureSizeInLuxels[0] + 1.0f + safeBorderWidth);
			res.UV1.Y = (res.UV1.Y + safeOffset) / ((float)f.LightmapTextureSizeInLuxels[1] + 1.0f + safeBorderWidth);

			SourceTexData tex = textures[(int)surf.texdata];
			res.UV0 = new Vector3(res.UV0.X / ((tex.width != 0) ? (float)tex.width : 256.0f), res.UV0.Y / ((tex.height != 0) ? (float)tex.height : 256.0f),0.0f);
		}

		public override string GameRootPath
		{
			get
			{
				return base.GameRootPath;
			}
			set
			{
				base.GameRootPath = value;
			}
		}
		private void BuildSubmeshes(int maxTextures, VertexBufferSubmesh[] submeshes, VertexBufferMesh streamMesh)
		{
			int[] textureToMaterial = new int[maxTextures];
			foreach (var quake3Face in this.faces)
			{
				++textureToMaterial[texInfo[quake3Face.texinfo].texdata];
			}
			for (int i = 0; i < maxTextures; ++i)
			{
				if (textureToMaterial[i] > 0)
				{
					submeshes[i] = streamMesh.CreateSubmesh() as VertexBufferSubmesh;

					int index = this.Scene.Materials.Count;
					var baseFileName = this.GetMaterialFileName(i);
					var imagePath = baseFileName;
					var texture = new FileReferenceImage { Path = imagePath };
					this.Scene.Images.Add(texture);
					var effect = new SceneEffect
						{
							//Diffuse = new ImageColorSource { Image = texture }
						};
					this.Scene.Effects.Add(effect);
					var sceneMaterial = new SceneMaterial { Effect = effect };
					this.Scene.Materials.Add(sceneMaterial);
					submeshes[i].Material = sceneMaterial;
					textureToMaterial[i] = index;
				}
			}
		}

		private string GetMaterialFileName(int i)
		{
			var rootPath = this.GameRootPath;
			var name = this.textures[i].name;
			var firstSubFolder = name.IndexOf('/');
			name = name.Substring(firstSubFolder + 1);
			return Path.GetFullPath(Path.Combine(Path.Combine(rootPath, "materials"), name) + ".vmt");
		}

		protected virtual void ReadEntry(ref SourceFileEntry entry)
		{
			entry.offset = this.Stream.ReadUInt32();
			entry.size = this.Stream.ReadUInt32();
			entry.version = this.Stream.ReadUInt32();
			entry.magic = this.Stream.ReadUInt32();
		}

		protected virtual void ReadFace(ref SourceFace face)
		{
			this.Stream.ReadUInt32();
			//source.ReadBytes(4);

			face.planenum = this.Stream.ReadUInt16(); // the plane number
			face.side = (byte)this.Stream.ReadByte(); // faces opposite to the node's plane direction
			face.onNode = (byte)this.Stream.ReadByte(); // 1 of on node, 0 if in leaf

			face.firstedge = this.Stream.ReadInt32(); // index into surfedges	
			face.numedges = this.Stream.ReadInt16(); // number of surfedges
			face.texinfo = this.Stream.ReadInt16(); // texture info
			face.dispinfo = this.Stream.ReadInt16(); // displacement info
			this.Stream.ReadBytes(50);
			face.origFace = this.Stream.ReadInt32(); // original face this was split from
			face.smoothingGroups = this.Stream.ReadUInt32(); // lightmap smoothing group
		}
		protected override void ReadPlanes()
		{
			this.SeekEntryAt(this.header.Planes.offset);
			int size = this.EvalNumItems(this.header.Planes.size, 20);
			this.planes = new SourcePlane[size];
			for (int i = 0; i < size; ++i)
			{
				this.ReadPlane(ref this.planes[i]);
			}
			this.AssertStreamPossition(this.header.Planes.size + this.header.Planes.offset);

		}

		private void ReadPlane(ref SourcePlane sourcePlane)
		{
			sourcePlane.normal.X = Stream.ReadSingle();
			sourcePlane.normal.Y = Stream.ReadSingle();
			sourcePlane.normal.Z = Stream.ReadSingle();
			sourcePlane.dist = Stream.ReadSingle();
			sourcePlane.type = Stream.ReadInt32();
		}

		protected override void ReadFaces()
		{
			this.SeekEntryAt(this.header.Faces.offset);
			int size = this.EvalNumItems(this.header.Faces.size, 104);
			this.faces = new SourceFace[size];
			for (int i = 0; i < size; ++i)
			{
				this.ReadFace(ref this.faces[i]);
			}
			this.AssertStreamPossition(this.header.Faces.size + this.header.Faces.offset);
		}

		protected override void ReadHeader()
		{
			this.header.magic = this.Stream.ReadUInt32();
			this.header.version = this.Stream.ReadUInt32();

			this.ReadEntry(ref this.header.Entities);
			this.ReadEntry(ref this.header.Planes); //Plane array
			this.ReadEntry(ref this.header.Texdata); //Index to texture names
			this.ReadEntry(ref this.header.Vertexes); //Vertex array
			this.ReadEntry(ref this.header.Visibility); //Compressed visibility bit arrays
			this.ReadEntry(ref this.header.Nodes); //BSP tree nodes  
			this.ReadEntry(ref this.header.Texinfo); //Face texture array
			this.ReadEntry(ref this.header.Faces); //Face array
			this.ReadEntry(ref this.header.Lighting); //Lightmap samples
			this.ReadEntry(ref this.header.Occlusion); //Occlusion data(?)
			this.ReadEntry(ref this.header.Leafs); //BSP tree leaf nodes
			this.ReadEntry(ref this.header.Unused11); //
			this.ReadEntry(ref this.header.Edges); //Edge array
			this.ReadEntry(ref this.header.Surfedges); //Index of edges
			this.ReadEntry(ref this.header.Models); //Brush models (geometry of brush entities)
			this.ReadEntry(ref this.header.Worldlights); //Light entities
			this.ReadEntry(ref this.header.LeafFaces); //Index to faces in each leaf
			this.ReadEntry(ref this.header.LeafBrushes); //Index to brushes in each leaf
			this.ReadEntry(ref this.header.Brushes); //Brush array
			this.ReadEntry(ref this.header.Brushsides); //Brushside array
			this.ReadEntry(ref this.header.Areas); //Area array
			this.ReadEntry(ref this.header.AreaPortals); //Portals between areas
			this.ReadEntry(ref this.header.Portals); //Polygons defining the boundary between adjacent leaves(?)
			this.ReadEntry(ref this.header.Clusters); //Leaves that are enterable by the player
			this.ReadEntry(ref this.header.PortalVerts); //Vertices of portal polygons
			this.ReadEntry(ref this.header.Clusterportals); //Polygons defining the boundary between adjacent clusters(?)
			this.ReadEntry(ref this.header.Dispinfo); //Displacement surface array
			this.ReadEntry(ref this.header.OriginalFaces); //Brush faces array before BSP splitting
			this.ReadEntry(ref this.header.Unused28); //
			this.ReadEntry(ref this.header.PhysCollide); //Physics collision data(?)
			this.ReadEntry(ref this.header.VertNormals); //Vertex normals(?)
			this.ReadEntry(ref this.header.VertNormalIndices); //Vertex normal index array(?)
			this.ReadEntry(ref this.header.DispLightmapAlphas); //Displacement lightmap data(?)
			this.ReadEntry(ref this.header.DispVerts); //Vertices of displacement surface meshes
			this.ReadEntry(ref this.header.DispLightmapSamplePos); //Displacement lightmap data(?)
			this.ReadEntry(ref this.header.GameLump); //Game-specific data lump
			this.ReadEntry(ref this.header.LeafWaterData); // (?)
			this.ReadEntry(ref this.header.Primitives); //Non-polygonal primatives(?)
			this.ReadEntry(ref this.header.PrimVerts); // (?)
			this.ReadEntry(ref this.header.PrimIndices); //(?)
			this.ReadEntry(ref this.header.Pakfile); //Embedded uncompressed-Zip format file
			this.ReadEntry(ref this.header.ClipPortalVerts); //(?)
			this.ReadEntry(ref this.header.Cubemaps); //Env_cubemap location array
			this.ReadEntry(ref this.header.TexdataStringData); //Texture name data
			this.ReadEntry(ref this.header.TexdataStringTable); //Index array into texdata string data
			this.ReadEntry(ref this.header.Overlays); //Info_overlay array       
			this.ReadEntry(ref this.header.LeafMinDistToWater); //(?)
			this.ReadEntry(ref this.header.FaceMacroTextureInfo); //(?)
			this.ReadEntry(ref this.header.DispTris); //Displacement surface triangles
			this.ReadEntry(ref this.header.PhysCollideSurface); //Physics collision surface data(?)
			this.ReadEntry(ref this.header.Unused50); //
			this.ReadEntry(ref this.header.Unused51); //
			this.ReadEntry(ref this.header.Unused52); //
			this.ReadEntry(ref this.header.LightingHDR); //HDR related lighting data(?)
			this.ReadEntry(ref this.header.WorldlightsHDR); //HDR related worldlight data(?)
			this.ReadEntry(ref this.header.LeaflightHDR1); //HDR related leaf lighting data(?)
			this.ReadEntry(ref this.header.LeaflightHDR2); //HDR related leaf lighting data(?)
			this.ReadEntry(ref this.header.Unused57);
			this.ReadEntry(ref this.header.Unused58);
			this.ReadEntry(ref this.header.Unused59);
			this.ReadEntry(ref this.header.Unused60);
			this.ReadEntry(ref this.header.Unused61);
			this.ReadEntry(ref this.header.Unused62);
			this.ReadEntry(ref this.header.Unused63);

			this.header.revision = this.Stream.ReadUInt32();
		}

		protected override void ReadTextures()
		{
			this.SeekEntryAt(this.header.Texdata.offset);
			int size = this.EvalNumItems(this.header.Texdata.size, 8 * 4);
			this.textures = new SourceTexData[size];
			for (int i = 0; i < size; ++i)
			{
				this.ReadTexture(ref this.textures[i]);
			}
			this.AssertStreamPossition(this.header.Texdata.size + this.header.Texdata.offset);

			this.SeekEntryAt(this.header.TexdataStringTable.offset);
			size = this.EvalNumItems(this.header.TexdataStringTable.size, 4);
			this.texdataStringTable = new uint[size];
			for (int i = 0; i < size; ++i)
			{
				this.texdataStringTable[i] = this.Stream.ReadUInt32();
			}
			this.AssertStreamPossition(this.header.TexdataStringTable.size + this.header.TexdataStringTable.offset);

			for (int i = 0; i < this.textures.Length; ++i)
			{
				this.SeekEntryAt(this.header.TexdataStringData.offset + this.texdataStringTable[this.textures[i].nameStringTableID]);
				this.textures[i].name = this.Stream.ReadStringZ();
			}

			this.SeekEntryAt(this.header.Texinfo.offset);
			size = this.EvalNumItems(this.header.Texinfo.size, 72);
			this.texInfo = new SourceTexInfo[size];
			for (int i = 0; i < size; ++i)
			{
				this.ReadTextureInfo(ref this.texInfo[i]);
			}
			this.AssertStreamPossition(this.header.Texinfo.size + this.header.Texinfo.offset);
		}

		private void ReadTextureInfo(ref SourceTexInfo sourceTexInfo)
		{
			sourceTexInfo.vectorS.X = Stream.ReadSingle();
			sourceTexInfo.vectorS.Y = Stream.ReadSingle();
			sourceTexInfo.vectorS.Z = Stream.ReadSingle();
			sourceTexInfo.distS = Stream.ReadSingle();

			sourceTexInfo.vectorT.X = Stream.ReadSingle();
			sourceTexInfo.vectorT.Y = Stream.ReadSingle();
			sourceTexInfo.vectorT.Z = Stream.ReadSingle();
			sourceTexInfo.distT = Stream.ReadSingle();

			sourceTexInfo.lm_vectorS.X = Stream.ReadSingle();
			sourceTexInfo.lm_vectorS.Y = Stream.ReadSingle();
			sourceTexInfo.lm_vectorS.Z = Stream.ReadSingle();
			sourceTexInfo.lm_distS = Stream.ReadSingle();

			sourceTexInfo.lm_vectorT.X = Stream.ReadSingle();
			sourceTexInfo.lm_vectorT.Y = Stream.ReadSingle();
			sourceTexInfo.lm_vectorT.Z = Stream.ReadSingle();
			sourceTexInfo.lm_distT = Stream.ReadSingle();

			sourceTexInfo.flags = Stream.ReadInt32();
			sourceTexInfo.texdata = Stream.ReadInt32();
		}
		protected override void ReadEdges()
		{
			this.SeekEntryAt(this.header.Edges.offset);
			var size = this.EvalNumItems(this.header.Edges.size, 4);
			this.edges = new SourceEdge[size];
			for (int i = 0; i < size; ++i)
			{
				this.ReadEdge(ref this.edges[i]);
			}
			this.AssertStreamPossition(this.header.Edges.size + this.header.Edges.offset);
		}

		private void ReadEdge(ref SourceEdge sourceEdge)
		{
			sourceEdge.vertex0 = this.Stream.ReadUInt16();
			sourceEdge.vertex1 = this.Stream.ReadUInt16();
		}

		protected override void ReadVertices()
		{
			this.SeekEntryAt(this.header.Vertexes.offset);
			int size = this.EvalNumItems(this.header.Vertexes.size, 12);
			this.vertices = new Vector3[size];
			for (int i = 0; i < size; ++i)
			{
				this.Stream.ReadVector3(out this.vertices[i]);
			}
			this.AssertStreamPossition(this.header.Vertexes.size + this.header.Vertexes.offset);

			
			this.SeekEntryAt(this.header.Surfedges.offset);
			size = this.EvalNumItems(this.header.Surfedges.size, 4);
			this.listOfEdges = new int[size];
			for (int i = 0; i < size; ++i)
			{
				this.listOfEdges[i] = this.Stream.ReadInt32();
			}
			this.AssertStreamPossition(this.header.Surfedges.size + this.header.Surfedges.offset);
		}

		private void ReadTexture(ref SourceTexData sourceTexData)
		{
			sourceTexData.reflectivity.X = this.Stream.ReadSingle();
			sourceTexData.reflectivity.Y = this.Stream.ReadSingle();
			sourceTexData.reflectivity.Z = this.Stream.ReadSingle();
			sourceTexData.nameStringTableID = this.Stream.ReadInt32();
			sourceTexData.width = this.Stream.ReadInt32();
			sourceTexData.height = this.Stream.ReadInt32();
			sourceTexData.view_width = this.Stream.ReadInt32();
			sourceTexData.view_height = this.Stream.ReadInt32();
		}

		#endregion
	}
}