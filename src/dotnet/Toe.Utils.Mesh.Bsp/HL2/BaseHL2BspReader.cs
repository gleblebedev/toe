using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

using Toe.Utils.Mesh.Bsp.Utils;
using Toe.Utils.ToeMath;

namespace Toe.Utils.Mesh.Bsp.HL2
{
	public class BaseHL2BspReader : BaseBspReader, IMaterialProvider
	{
		#region Constants and Fields

		protected SourceFace[] faces;

		protected SourceFileHeader header;

		protected SourceTexData[] textures;

		private bool buildBsp;

		private SourceCluster[] clusters;

		private SourceEdge[] edges;

		private string entitiesInfo;

		private ushort[] leafFaces;

		private SourceLeaf[] leaves;

		private int[] listOfEdges;

		private SourceModel[] models;

		private SourceNode[] nodes;

		private SourcePlane[] planes;

		private float safeBorderWidth = 1.0f;

		private float safeOffset = 0.5f; //0.5f;

		private SourceTexInfo[] texInfo;

		private uint[] texdataStringTable;

		private Float3[] vertices;

		private SeparateStreamsMesh streamMesh;

		#endregion

		#region Public Properties

		public BaseHL2BspReader(IStreamConverterFactory streamConverterFactory)
			: base(streamConverterFactory)
		{
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

		#endregion

		#region Public Methods and Operators

		public IMaterial CreateMaterial(BspMaterialKey material)
		{
			var baseFileName = this.GetMaterialFileName(material.Material);
			var imagePath = baseFileName;
			var texture = new FileReferenceImage { Path = imagePath };
			this.Scene.Images.Add(texture);
			var effect = new SceneEffect
				{
					CullMode = CullMode.None,
					//Diffuse = new ImageColorSource { Image = texture }
				};
			this.Scene.Effects.Add(effect);
			var sceneMaterial = new SceneMaterial { Effect = effect };
			this.Scene.Materials.Add(sceneMaterial);
			return sceneMaterial;
		}

		#endregion

		#region Methods

		protected override void CreateMesh()
		{
			this.streamMesh = new SeparateStreamsMesh();
			this.meshStreams = new BspMeshStreams();
			meshStreams.Positions = streamMesh.SetStream(Streams.Position, 0, new ListMeshStream<Float3>(StreamConverterFactory));
			meshStreams.Normals = streamMesh.SetStream(Streams.Normal, 0, new ListMeshStream<Float3>(StreamConverterFactory));
			meshStreams.TexCoord0 = streamMesh.SetStream(Streams.TexCoord, 0, new ListMeshStream<Float2>(StreamConverterFactory));
			meshStreams.TexCoord1 = streamMesh.SetStream(Streams.TexCoord, 1, new ListMeshStream<Float2>(StreamConverterFactory));
			meshStreams.Colors = streamMesh.SetStream(Streams.Color, 0, new ListMeshStream<Color>(StreamConverterFactory));
		}

		protected override void BuildScene()
		{
			this.CollectLeafsInCluster();
			//BuildVisibilityList();

			var node = new Node();
			this.Scene.Nodes.Add(node);

			if (this.buildBsp)
			{
				this.Scene.Geometries.Add(streamMesh);
				var meshBuilder = new MeshBuilder(streamMesh, this);
				node.Mesh = streamMesh;

				var vsdNodes = new BspVsdTreeNode[this.nodes.Length];
				for (int index = 0; index < this.nodes.Length; index++)
				{
					var sourceNode = this.nodes[index];
					vsdNodes[index] = new BspVsdTreeNode
						{
							Min = new Float3(sourceNode.box.boxMinX, sourceNode.box.boxMinY, sourceNode.box.boxMinZ),
							Max = new Float3(sourceNode.box.boxMaxX, sourceNode.box.boxMaxY, sourceNode.box.boxMaxZ),
							PositiveNodeIndex = sourceNode.front,
							NegativeNodeIndex = sourceNode.back,
							N = this.planes[sourceNode.planenum].normal,
							D = this.planes[sourceNode.planenum].dist,
						};
					if (sourceNode.face_num > 0)
					{
						//TODO: put this faces into geometry
						sourceNode.face_num = sourceNode.face_num;
					}
				}

				List<int> visibleClustersLookup = new List<int>();
				List<int> visibleMeshesLookup = new List<int>();
				var vsdLeaves = new BspVsdTreeLeaf[this.leaves.Length];
				var vsdClusters = new BspVsdTreeCluster[this.clusters.Length];
				//int nodesMeshId;
				//var nodesMesh = meshBuilder.EnsureSubMesh(new BspSubmeshKey(-1, new BspMaterialKey(0, 0)), out nodesMeshId);
				//for (int index = 0; index < this.nodes.Length; index++)
				//{
				//    for (int j = nodes[index].face_id; j < nodes[index].face_id + nodes[index].face_num;++j )
				//        this.BuildFace(ref this.faces[j], nodesMesh, streamMesh);
				//}
				for (int index = 0; index < this.leaves.Length; index++)
				{
					var sourceLeaf = this.leaves[index];
					vsdLeaves[index] = new BspVsdTreeLeaf
						{
							Min = new Float3(sourceLeaf.box.boxMinX, sourceLeaf.box.boxMinY, sourceLeaf.box.boxMinZ),
							Max = new Float3(sourceLeaf.box.boxMaxX, sourceLeaf.box.boxMaxY, sourceLeaf.box.boxMaxZ),
							Cluster = sourceLeaf.cluster,
						};
				}
				for (int index = 0; index < this.clusters.Length; index++)
				{
					Dictionary<int, bool> uniqueSubmeshes = new Dictionary<int, bool>();
					var sourceCluster = this.clusters[index];
					vsdClusters[index] = new BspVsdTreeCluster
						{
							VisibleClustersCount = sourceCluster.visiblity.Count,
							VisibleClustersOffset = visibleClustersLookup.Count,
							VisibleMeshesOffset = visibleMeshesLookup.Count,
						};

					visibleClustersLookup.AddRange(sourceCluster.visiblity);

					Dictionary<int, bool> uniqueFaces = new Dictionary<int, bool>();

					foreach (var leafIndex in sourceCluster.leaves)
					{
						var sourceLeaf = this.leaves[leafIndex];
						var faceBegin = sourceLeaf.firstleafface;
						var faceEnd = faceBegin + sourceLeaf.numleaffaces;
						for (int f = faceBegin; f < faceEnd; ++f)
						{
							uniqueFaces[this.leafFaces[f]] = true;
						}
					}
					foreach (var uniqueFace in uniqueFaces)
					{
						int texIndex = 0; // this.texInfo[this.faces[faceIndex].texinfo].texdata;
						int lightmapIndex = 0; //this.faces[faceIndex].lightmap;

						int meshIndex;
						SeparateStreamsSubmesh subMesh =
							meshBuilder.EnsureSubMesh(new BspSubmeshKey(index, new BspMaterialKey(texIndex, lightmapIndex)), out meshIndex);
						if (!uniqueSubmeshes.ContainsKey(meshIndex))
						{
							visibleMeshesLookup.Add(meshIndex);
							uniqueSubmeshes[meshIndex] = true;
						}
						BspSubmeshStreams submeshStreams= new BspSubmeshStreams(subMesh, meshStreams,StreamConverterFactory);
						this.BuildFace(ref this.faces[uniqueFace.Key], submeshStreams);
					}
					vsdClusters[index].VisibleMeshesCount = visibleMeshesLookup.Count - vsdClusters[index].VisibleMeshesOffset;
				}

				//for (int index = 0; index < usedFaces.Length; index++)
				//{
				//    if (!usedFaces[index])
				//    {
				//        for (int i = 0; i < this.models.Length; i++)
				//        {
				//            if (this.models[i].firstface <= index && this.models[i].firstface+this.models[i].numfaces > index)
				//            {
				//                Trace.WriteLine(string.Format("Lost face {0} belongs to model {1}", index, i));
				//                break;
				//            }
				//        }
				//        for (int i = 0; i < this.nodes.Length; i++)
				//        {
				//            if (this.nodes[i].face_id <= index && this.nodes[i].face_id + this.nodes[i].face_num > index)
				//            {
				//                Trace.WriteLine(string.Format("Lost face {0} belongs to node {1}", index, i));
				//                break;
				//            }
				//        }
				//        for (int i = 0; i < this.leaves.Length; i++)
				//        {
				//            var begin = this.leaves[i].firstleafface;
				//            var end = begin+this.leaves[i].numleaffaces;
				//            while (begin<end)
				//            {
				//                if (leafFaces[begin] == index)
				//                {
				//                    Trace.WriteLine(string.Format("Lost face {0} belongs to leaf {1}", index, i));
				//                    break;
				//                }
				//                ++begin;
				//            }
				//        }
				//    }
				//}

				this.Scene.VsdProvider = new BspVsdProvider
					{
						VisibleClustersLookupTable = visibleClustersLookup.ToArray(),
						VisibleMeshesLookupTable = visibleMeshesLookup.ToArray(),
						Clusters = vsdClusters,
						Leaves = vsdLeaves,
						Models = (from model in this.models select new BspVsdTreeModel { RootNode = model.headnode }).ToArray(),
						Level = streamMesh,
						Nodes = vsdNodes
					};

				this.BuildAdditionalNodes();
			}
			else
			{
				this.BuildAdditionalNodes();
				node.Mesh = this.Scene.Geometries[0];
			}

			this.BuildEntityNodes(this.entitiesInfo);
		}

		protected override object ConvertEntityProperty(string key, string val)
		{
			switch (key)
			{
				default:
					return base.ConvertEntityProperty(key, val);
			}
		}

		protected void ReadBBox(ref SourceBoundingBox box)
		{
			box.boxMinX = this.Stream.ReadInt16();
			box.boxMinY = this.Stream.ReadInt16();
			box.boxMinZ = this.Stream.ReadInt16();
			box.boxMaxX = this.Stream.ReadInt16();
			box.boxMaxY = this.Stream.ReadInt16();
			box.boxMaxZ = this.Stream.ReadInt16();
		}

		protected void ReadCompressedLightCube(ref SourceCompressedLightCube ambientLighting)
		{
			ambientLighting.UnknownData0 = this.Stream.ReadUInt32();
			ambientLighting.UnknownData1 = this.Stream.ReadUInt32();
			ambientLighting.UnknownData2 = this.Stream.ReadUInt32();
			ambientLighting.UnknownData3 = this.Stream.ReadUInt32();
			ambientLighting.UnknownData4 = this.Stream.ReadUInt32();
			ambientLighting.UnknownData5 = this.Stream.ReadUInt32();
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

		protected override void ReadEntities()
		{
			this.SeekEntryAt(this.header.Entities.offset);
			this.entitiesInfo =
				Encoding.UTF8.GetString(this.Stream.ReadBytes((int)this.header.Entities.size)).Trim(new[] { '\0' });
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

		protected virtual void ReadLeaf(ref SourceLeaf sourceLeaf)
		{
			sourceLeaf.contents = this.Stream.ReadInt32(); // OR of all brushes (not needed?)
			sourceLeaf.cluster = this.Stream.ReadInt16(); // cluster this leaf is in
			sourceLeaf.area_flags = this.Stream.ReadInt16(); // area this leaf is in
			this.ReadBBox(ref sourceLeaf.box);
			sourceLeaf.firstleafface = this.Stream.ReadUInt16(); // index into leaffaces
			sourceLeaf.numleaffaces = this.Stream.ReadUInt16();
			sourceLeaf.firstleafbrush = this.Stream.ReadUInt16(); // index into leafbrushes
			sourceLeaf.numleafbrushes = this.Stream.ReadUInt16();
			sourceLeaf.leafWaterDataID = this.Stream.ReadInt16(); // -1 for not in water
			sourceLeaf.padding = this.Stream.ReadInt16(); // padding to 4-byte boundary
		}

		protected override void ReadLeaves()
		{
			this.ReadLeaves(32);
		}

		protected void ReadLeaves(int itemSize)
		{
			this.SeekEntryAt(this.header.Leafs.offset);
			var size = this.EvalNumItems(this.header.Leafs.size, itemSize);
			this.leaves = new SourceLeaf[size];
			for (int i = 0; i < size; ++i)
			{
				this.ReadLeaf(ref this.leaves[i]);
			}
			this.AssertStreamPossition(this.header.Leafs.size + this.header.Leafs.offset);

			this.SeekEntryAt(this.header.LeafFaces.offset);
			size = this.EvalNumItems(this.header.LeafFaces.size, 2);
			this.leafFaces = new ushort[size];
			for (int i = 0; i < size; ++i)
			{
				this.leafFaces[i] = this.Stream.ReadUInt16();
			}
			this.AssertStreamPossition(this.header.LeafFaces.size + this.header.LeafFaces.offset);
		}

		protected override void ReadModels()
		{
			this.SeekEntryAt(this.header.Models.offset);
			int size = this.EvalNumItems(this.header.Models.size, 12 * 4);
			this.models = new SourceModel[size];
			for (int i = 0; i < size; ++i)
			{
				this.ReadModel(ref this.models[i]);
			}
			this.AssertStreamPossition(this.header.Models.size + this.header.Models.offset);
		}

		protected virtual void ReadNode(ref SourceNode sourceNode)
		{
			sourceNode.planenum = this.Stream.ReadInt32();
			sourceNode.front = this.Stream.ReadInt32();
			sourceNode.back = this.Stream.ReadInt32();
			this.ReadBBox(ref sourceNode.box);
			sourceNode.face_id = this.Stream.ReadUInt16();
			sourceNode.face_num = this.Stream.ReadUInt16();
			sourceNode.area = this.Stream.ReadInt16();
			sourceNode.paddding = this.Stream.ReadInt16();
		}

		protected override void ReadNodes()
		{
			this.SeekEntryAt(this.header.Nodes.offset);
			var size = this.EvalNumItems(this.header.Nodes.size, 32);
			this.nodes = new SourceNode[size];
			for (int i = 0; i < size; ++i)
			{
				this.ReadNode(ref this.nodes[i]);
			}
			this.AssertStreamPossition(this.header.Nodes.size + this.header.Nodes.offset);
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

		protected override void ReadVertices()
		{
			this.SeekEntryAt(this.header.Vertexes.offset);
			int size = this.EvalNumItems(this.header.Vertexes.size, 12);
			this.vertices = new Float3[size];
			for (int i = 0; i < size; ++i)
			{
				this.Stream.ReadFloat3(out this.vertices[i]);
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

		protected override void ReadVisibilityList()
		{
			this.SeekEntryAt(this.header.Visibility.offset);
			var pos = this.Stream.Position;
			int num_clusters = this.Stream.ReadInt32();
			this.clusters = new SourceCluster[num_clusters];
			for (int i = 0; i < num_clusters; ++i)
			{
				this.clusters[i].offset = this.Stream.ReadInt32();
				this.clusters[i].phs = this.Stream.ReadInt32();
				this.clusters[i].visiblity = new List<int>();
				this.clusters[i].leaves = new List<int>();
			}
			for (int i = 0; i < num_clusters; ++i)
			{
				this.Stream.Position = pos + this.clusters[i].offset;
				for (int c = 0; c < num_clusters;)
				{
					byte pvs_buffer = (byte)this.Stream.ReadByte();
					if (pvs_buffer == 0)
					{
						c += 8 * (byte)this.Stream.ReadByte();
					}
					else
					{
						for (byte bit = 1; bit != 0; bit *= 2, c++)
						{
							if (0 != (pvs_buffer & bit))
							{
								if (c < 0)
								{
									throw new BspFormatException(string.Format("Cluster index is {0}", c));
								}
								this.clusters[i].visiblity.Add(c);
							}
						}
					}
				}
			}
		}

		private void BuildAdditionalNodes()
		{
			for (int i = this.buildBsp ? 1 : 0; i < this.models.Length; ++i)
			{
				this.BuildModelAsNode(ref this.models[i]);
			}
		}

		private void BuildFace(ref SourceFace face, BspSubmeshStreams subMesh)
		{
			Vertex[] faceVertices = new Vertex[face.numedges];

			var plane = this.planes[face.planenum];
			var texture_id = this.texInfo[face.texinfo].texdata;

			Float2 minUV0 = new Float2(float.MaxValue, float.MaxValue);
			Float2 minUV1 = new Float2(float.MaxValue, float.MaxValue);
			Float2 maxUV1 = new Float2(float.MinValue, float.MinValue);
			int nextShouldBe = -1;
			for (int index = 0; index < faceVertices.Length; index++)
			{
				var listOfEdgesIndex = face.firstedge + index;
				if (listOfEdgesIndex >= this.listOfEdges.Length)
				{
					throw new BspFormatException(
						string.Format("Edge list index {0} is out of range [0..{1}]", listOfEdgesIndex, this.listOfEdges.Length - 1));
				}

				var edgeIndex = this.listOfEdges[listOfEdgesIndex];
				if (edgeIndex >= this.edges.Length)
				{
					throw new BspFormatException(
						string.Format("Edge index {0} is out of range [0..{1}]", edgeIndex, this.edges.Length - 1));
				}

				SourceEdge edge;
				if (edgeIndex >= 0)
				{
					edge = this.edges[edgeIndex];
				}
				else
				{
					var flippedEdge = this.edges[-edgeIndex];
					edge = new SourceEdge { vertex0 = flippedEdge.vertex1, vertex1 = flippedEdge.vertex0 };
				}
				var edgesvertex0 = edge.vertex0;
				if (edgesvertex0 >= this.vertices.Length)
				{
					throw new BspFormatException(
						string.Format("Vertex index {0} is out of range [0..{1}]", edgesvertex0, this.vertices.Length - 1));
				}
				var edgesvertex1 = edge.vertex1;
				if (edgesvertex1 >= this.vertices.Length)
				{
					throw new BspFormatException(
						string.Format("Vertex index {0} is out of range [0..{1}]", edgesvertex1, this.vertices.Length - 1));
				}
				if (nextShouldBe >= 0 && nextShouldBe != edgesvertex0)
				{
					throw new BspFormatException(string.Format("Wrong edge order"));
				}
				nextShouldBe = edgesvertex1;
				Vertex vertex;
				this.BuildVertex(
					this.vertices[(short)edgesvertex0],
					plane.normal,
					//(face.side == 0) ? plane.normal : -plane.normal,
					face,
					ref this.texInfo[face.texinfo],
					out vertex);
				faceVertices[index] = vertex;
				if (minUV0.X > vertex.UV0.X)
				{
					minUV0.X = vertex.UV0.X;
				}
				if (minUV0.Y > vertex.UV0.Y)
				{
					minUV0.Y = vertex.UV0.Y;
				}
				if (minUV1.X > vertex.UV1.X)
				{
					minUV1.X = vertex.UV1.X;
				}
				if (minUV1.Y > vertex.UV1.Y)
				{
					minUV1.Y = vertex.UV1.Y;
				}
				if (maxUV1.X < vertex.UV1.X)
				{
					maxUV1.X = vertex.UV1.X;
				}
				if (maxUV1.Y < vertex.UV1.Y)
				{
					maxUV1.Y = vertex.UV1.Y;
				}
			}
			if (this.textures[texture_id].name == "TOOLS/TOOLSSKYBOX")
			{
				minUV0.X = 0;
				minUV0.Y = 0;
				for (int j = 0; j < (int)face.numedges; ++j)
				{
					faceVertices[j].UV0 = new Float3(0, 0, 0);
				}
			}

			int[] indices = new int[faceVertices.Length];
			for (int j = 0; j < faceVertices.Length; ++j)
			{
				meshStreams.Positions.Add(faceVertices[j].Position);
				meshStreams.Normals.Add(faceVertices[j].Normal);
				meshStreams.Colors.Add(faceVertices[j].Color);
				meshStreams.TexCoord0.Add(new Float2(faceVertices[j].UV0.X, faceVertices[j].UV0.Y));
				meshStreams.TexCoord1.Add(new Float2(faceVertices[j].UV1.X, faceVertices[j].UV1.Y));
			}
			for (int j = 1; j < faceVertices.Length - 1; ++j)
			{
				subMesh.AddToAllStreams(indices[0]);
				subMesh.AddToAllStreams(indices[j]);
				subMesh.AddToAllStreams(indices[j + 1]);
			}
		}

		private void BuildModelAsNode(ref SourceModel sourceModel)
		{
			var streamMesh2 = new SeparateStreamsMesh();
			var meshBuilder2 = new MeshBuilder(streamMesh2, this);
			var beginModelFace = sourceModel.firstface;
			var endModelFace = beginModelFace + sourceModel.numfaces;
			for (int index = beginModelFace; index < endModelFace; ++index)
			{
				//if (!usedFaces[index])
				{
					int meshIndex;
					SeparateStreamsSubmesh subMesh2 = meshBuilder2.EnsureSubMesh(
						new BspSubmeshKey(0, new BspMaterialKey(0, 0)), out meshIndex);
					this.BuildFace(ref this.faces[index], new BspSubmeshStreams(subMesh2,meshStreams,StreamConverterFactory));
					//Trace.WriteLine(string.Format("Face {0} is not references from leaves", index));
				}
			}
			this.Scene.Geometries.Add(streamMesh2);
		}

		//private void BuildSubmeshes(int maxTextures)
		//{
		//    int[] textureToMaterial = new int[maxTextures];
		//    foreach (var quake3Face in this.faces)
		//    {
		//        ++textureToMaterial[this.texInfo[quake3Face.texinfo].texdata];
		//    }
		//    for (int i = 0; i < maxTextures; ++i)
		//    {
		//        if (textureToMaterial[i] > 0)
		//        {
		//            int index = this.Scene.Materials.Count;
		//            var baseFileName = this.GetMaterialFileName(i);
		//            var imagePath = baseFileName;
		//            var texture = new FileReferenceImage { Path = imagePath };
		//            this.Scene.Images.Add(texture);
		//            var effect = new SceneEffect
		//                {
		//                    //Diffuse = new ImageColorSource { Image = texture }
		//                };
		//            this.Scene.Effects.Add(effect);
		//            var sceneMaterial = new SceneMaterial { Effect = effect };
		//            this.Scene.Materials.Add(sceneMaterial);
		//            submeshes[i].Material = sceneMaterial;
		//            textureToMaterial[i] = index;
		//        }
		//    }
		//}
		//private void BuildVisibilityList()
		//{
		//    for (int i = 0; i < leaves.Length; ++i)
		//    {
		//        Dictionary<int, bool> map = new Dictionary<int, bool>();
		//        if (leaves[i].cluster >= 0)
		//            foreach (var c in clusters[leaves[i].cluster].visiblity)
		//                foreach (var l in clusters[c].lists)
		//                    map[l] = true;
		//        leaves[i].VisibleLeaves = new List<int>();
		//        foreach (var j in map.Keys)
		//            if (i != j)
		//                leaves[i].VisibleLeaves.Add(j);
		//    }
		//}
		private void BuildVertex(Float3 vector3, Float3 n, SourceFace f, ref SourceTexInfo surf, out Vertex res)
		{
			res = new Vertex
				{
					Position = vector3,
					Normal = n,
					Color = Color.White,
					UV0 =
						new Float3(
						Float3.Dot(surf.vectorS, vector3) + surf.distS, Float3.Dot(surf.vectorT, vector3) + surf.distT, 0.0f),
					UV1 =
						new Float3(
						Float3.Dot(surf.lm_vectorS, vector3) + surf.lm_distS - f.LightmapTextureMinsInLuxels[0],
						Float3.Dot(surf.lm_vectorT, vector3) + surf.lm_distT - f.LightmapTextureMinsInLuxels[1],
						0.0f)
				};
			//if (f.LightmapTextureSizeInLuxels[0] == 0)
			res.UV1.X = (res.UV1.X + this.safeOffset) / (f.LightmapTextureSizeInLuxels[0] + 1.0f + this.safeBorderWidth);
			res.UV1.Y = (res.UV1.Y + this.safeOffset) / (f.LightmapTextureSizeInLuxels[1] + 1.0f + this.safeBorderWidth);

			SourceTexData tex = this.textures[surf.texdata];
			res.UV0 = new Float3(
				res.UV0.X / ((tex.width != 0) ? tex.width : 256.0f), res.UV0.Y / ((tex.height != 0) ? tex.height : 256.0f), 0.0f);
		}

		private void CollectLeafsInCluster()
		{
			for (int i = 0; i < this.leaves.Length; ++i)
			{
				if (this.leaves[i].cluster >= 0)
				{
					this.clusters[this.leaves[i].cluster].leaves.Add(i);
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

		private void ReadEdge(ref SourceEdge sourceEdge)
		{
			sourceEdge.vertex0 = this.Stream.ReadUInt16();
			sourceEdge.vertex1 = this.Stream.ReadUInt16();
		}

		private void ReadModel(ref SourceModel sourceModel)
		{
			float x, y, z;
			x = this.Stream.ReadSingle();
			y = this.Stream.ReadSingle();
			z = this.Stream.ReadSingle();
			sourceModel.mins = new Float3(x, y, z);
			x = this.Stream.ReadSingle();
			y = this.Stream.ReadSingle();
			z = this.Stream.ReadSingle();
			sourceModel.maxs = new Float3(x, y, z);
			x = this.Stream.ReadSingle();
			y = this.Stream.ReadSingle();
			z = this.Stream.ReadSingle();
			sourceModel.origin = new Float3(x, y, z);
			sourceModel.headnode = this.Stream.ReadInt32();
			sourceModel.firstface = this.Stream.ReadInt32();
			sourceModel.numfaces = this.Stream.ReadInt32();
		}

		private void ReadPlane(ref SourcePlane sourcePlane)
		{
			sourcePlane.normal.X = this.Stream.ReadSingle();
			sourcePlane.normal.Y = this.Stream.ReadSingle();
			sourcePlane.normal.Z = this.Stream.ReadSingle();
			sourcePlane.dist = this.Stream.ReadSingle();
			sourcePlane.type = this.Stream.ReadInt32();
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

		private void ReadTextureInfo(ref SourceTexInfo sourceTexInfo)
		{
			sourceTexInfo.vectorS.X = this.Stream.ReadSingle();
			sourceTexInfo.vectorS.Y = this.Stream.ReadSingle();
			sourceTexInfo.vectorS.Z = this.Stream.ReadSingle();
			sourceTexInfo.distS = this.Stream.ReadSingle();

			sourceTexInfo.vectorT.X = this.Stream.ReadSingle();
			sourceTexInfo.vectorT.Y = this.Stream.ReadSingle();
			sourceTexInfo.vectorT.Z = this.Stream.ReadSingle();
			sourceTexInfo.distT = this.Stream.ReadSingle();

			sourceTexInfo.lm_vectorS.X = this.Stream.ReadSingle();
			sourceTexInfo.lm_vectorS.Y = this.Stream.ReadSingle();
			sourceTexInfo.lm_vectorS.Z = this.Stream.ReadSingle();
			sourceTexInfo.lm_distS = this.Stream.ReadSingle();

			sourceTexInfo.lm_vectorT.X = this.Stream.ReadSingle();
			sourceTexInfo.lm_vectorT.Y = this.Stream.ReadSingle();
			sourceTexInfo.lm_vectorT.Z = this.Stream.ReadSingle();
			sourceTexInfo.lm_distT = this.Stream.ReadSingle();

			sourceTexInfo.flags = this.Stream.ReadInt32();
			sourceTexInfo.texdata = this.Stream.ReadInt32();
		}

		#endregion
	}
}