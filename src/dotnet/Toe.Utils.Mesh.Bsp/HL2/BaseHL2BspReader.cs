using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;

using OpenTK;

using Toe.Utils.Mesh.Bsp.Utils;

namespace Toe.Utils.Mesh.Bsp.HL2
{
	public class BaseHL2BspReader : BaseBspReader, IMaterialProvider
	{
		#region Constants and Fields

		protected SourceFace[] faces;

		protected SourceFileHeader header;

		protected SourceTexData[] textures;

		private SourceEdge[] edges;

		private int[] listOfEdges;

		private SourcePlane[] planes;

		private float safeBorderWidth = 1.0f;

		private float safeOffset = 0.5f; //0.5f;

		private SourceTexInfo[] texInfo;

		private uint[] texdataStringTable;

		private Vector3[] vertices;

		private SourceNode[] nodes;

		private SourceLeaf[] leaves;

		private SourceModel[] models;

		private SourceCluster[] clusters;

		private ushort[] leafFaces;

		#endregion

		#region Public Properties

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

		#region Methods

		protected override void BuildScene()
		{
			CollectLeafsInCluster();
			//BuildVisibilityList();

			var maxTextures = this.textures.Length;
			var streamMesh = new VertexBufferMesh();
			var meshBuilder = new MeshBuilder(streamMesh,this);
			//this.BuildSubmeshes(maxTextures, submeshes, streamMesh);

			var node = new Node();
			this.Scene.Nodes.Add(node);
			node.Mesh = streamMesh;

			var vsdNodes = new BspVsdTreeNode[nodes.Length];
			for (int index = 0; index < this.nodes.Length; index++)
			{
				var sourceNode = this.nodes[index];
				vsdNodes[index] = new BspVsdTreeNode
					{
						Min = new Vector3(sourceNode.box.boxMinX, sourceNode.box.boxMinY, sourceNode.box.boxMinZ),
						Max = new Vector3(sourceNode.box.boxMaxX, sourceNode.box.boxMaxY, sourceNode.box.boxMaxZ),
						PositiveNodeIndex = sourceNode.front,
						NegativeNodeIndex = sourceNode.back,
						N = planes[sourceNode.planenum].normal,
						D = planes[sourceNode.planenum].dist,
					};
				if (sourceNode.face_num > 0)
				{
					//TODO: put this faces into geometry
					sourceNode.face_num = sourceNode.face_num;
				}
			}
			bool[] usedFaces = new bool[this.faces.Length];

			foreach (var usedFace in leafFaces)
			{
				usedFaces[usedFace] = true;
			}
			for (int index = 0; index < usedFaces.Length; index++)
			{
				if (!usedFaces[index])
				{
					Trace.WriteLine(string.Format("Face {0} is not references from leaves", index));
				}
			}

			List<int> visibleClustersLookup = new List<int>();
			List<int> visibleMeshesLookup = new List<int>();
			var vsdLeaves = new BspVsdTreeLeaf[leaves.Length];
			var vsdClusters = new BspVsdTreeCluster[clusters.Length];
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
				vsdLeaves[index] = new BspVsdTreeLeaf()
					{
						Min = new Vector3(sourceLeaf.box.boxMinX, sourceLeaf.box.boxMinY, sourceLeaf.box.boxMinZ),
						Max = new Vector3(sourceLeaf.box.boxMaxX, sourceLeaf.box.boxMaxY, sourceLeaf.box.boxMaxZ),
						Cluster = sourceLeaf.cluster,
					};
			}
			Dictionary<int, bool> uniqueSubmeshes = new Dictionary<int, bool>();
			for (int index = 0; index < this.clusters.Length; index++)
			{
				var sourceCluster = this.clusters[index];
				vsdClusters[index] = new BspVsdTreeCluster()
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
						uniqueFaces[leafFaces[f]] = true;
					}
				}
				foreach (var uniqueFace in uniqueFaces)
				{
					int texIndex = 0;// this.texInfo[this.faces[faceIndex].texinfo].texdata;
					int lightmapIndex = 0;//this.faces[faceIndex].lightmap;

					int meshIndex;
					VertexBufferSubmesh subMesh = meshBuilder.EnsureSubMesh(new BspSubmeshKey(index, new BspMaterialKey(texIndex, lightmapIndex)), out meshIndex);
					if (!uniqueSubmeshes.ContainsKey(meshIndex))
					{
						visibleMeshesLookup.Add(meshIndex);
						uniqueSubmeshes[meshIndex] = true;
					}
					this.BuildFace(ref this.faces[uniqueFace.Key], subMesh, streamMesh);
					usedFaces[uniqueFace.Key] = true;
				}
				vsdClusters[index].VisibleMeshesCount = visibleMeshesLookup.Count - vsdClusters[index].VisibleMeshesOffset;
			}

			for (int index = 0; index < usedFaces.Length; index++)
			{
				if (!usedFaces[index])
				{
					for (int i = 0; i < this.models.Length; i++)
					{
						if (this.models[i].firstface <= index && this.models[i].firstface+this.models[i].numfaces > index)
						{
							Trace.WriteLine(string.Format("Lost face {0} belongs to model {1}", index, i));
							break;
						}
					}
					for (int i = 0; i < this.nodes.Length; i++)
					{
						if (this.nodes[i].face_id <= index && this.nodes[i].face_id + this.nodes[i].face_num > index)
						{
							Trace.WriteLine(string.Format("Lost face {0} belongs to node {1}", index, i));
							break;
						}
					}
					for (int i = 0; i < this.leaves.Length; i++)
					{
						var begin = this.leaves[i].firstleafface;
						var end = begin+this.leaves[i].numleaffaces;
						while (begin<end)
						{
							if (leafFaces[begin] == index)
							{
								Trace.WriteLine(string.Format("Lost face {0} belongs to leaf {1}", index, i));
								break;
							}
							++begin;
						}
					}
				}
			}

			this.Scene.VsdProvider = new BspVsdProvider()
				{
					VisibleClustersLookupTable = visibleClustersLookup.ToArray(),
					VisibleMeshesLookupTable = visibleMeshesLookup.ToArray(),
					Clusters = vsdClusters,
					Leaves = vsdLeaves,
					Models = (from model in models  select new BspVsdTreeModel{RootNode = model.headnode}).ToArray(),
					Level = streamMesh, 
					Nodes = vsdNodes
				};

			this.Scene.Geometries.Add(streamMesh);

			int sumFaces = streamMesh.Submeshes.Sum(submesh => submesh.Count / 3);
			int sumFaces2 = this.faces.Sum(sourceFace => sourceFace.numedges - 2);
		}

		private void CollectLeafsInCluster()
		{
			for (int i = 0; i < leaves.Length; ++i)
			{
				if (leaves[i].cluster >= 0)
					clusters[leaves[i].cluster].leaves.Add(i);
			}
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
		protected override void ReadLeaves()
		{
			this.ReadLeaves(32);
		}
		protected override void ReadVisibilityList()
		{
			SeekEntryAt(this.header.Visibility.offset);
			var pos = Stream.Position;
			int num_clusters = Stream.ReadInt32();
			clusters = new SourceCluster[num_clusters];
			for (int i = 0; i < num_clusters; ++i)
			{
				clusters[i].offset = Stream.ReadInt32();
				clusters[i].phs = Stream.ReadInt32();
				clusters[i].visiblity = new List<int>();
				clusters[i].leaves = new List<int>();
			}
			for (int i = 0; i < num_clusters; ++i)
			{
				
				this.Stream.Position = pos + clusters[i].offset;
				for (int c = 0; c < num_clusters; )
				{
					byte pvs_buffer = (byte)Stream.ReadByte();
					if (pvs_buffer == 0)
					{
						c += 8 * (byte)Stream.ReadByte();
					}
					else
					{
						for (byte bit = 1; bit != 0; bit *= 2, c++)
						{
							if (0 != (pvs_buffer & bit))
							{
								if (c < 0)
									throw new BspFormatException(string.Format("Cluster index is {0}", c));
								clusters[i].visiblity.Add(c);
							}
						}
					}

				}
			}
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
				leafFaces[i] = Stream.ReadUInt16();
			}
			this.AssertStreamPossition(this.header.LeafFaces.size + this.header.LeafFaces.offset);
		}

		protected virtual void ReadLeaf(ref SourceLeaf sourceLeaf)
		{
			sourceLeaf.contents = Stream.ReadInt32();               // OR of all brushes (not needed?)
			sourceLeaf.cluster = Stream.ReadInt16();               // cluster this leaf is in
			sourceLeaf.area_flags = Stream.ReadInt16();                 // area this leaf is in
			this.ReadBBox(ref sourceLeaf.box);
			sourceLeaf.firstleafface = Stream.ReadUInt16();          // index into leaffaces
			sourceLeaf.numleaffaces = Stream.ReadUInt16();
			sourceLeaf.firstleafbrush = Stream.ReadUInt16();         // index into leafbrushes
			sourceLeaf.numleafbrushes = Stream.ReadUInt16();
			sourceLeaf.leafWaterDataID = Stream.ReadInt16();        // -1 for not in water
			sourceLeaf.padding = Stream.ReadInt16();                // padding to 4-byte boundary
		}
		protected void ReadCompressedLightCube(ref SourceCompressedLightCube ambientLighting)
		{
			ambientLighting.UnknownData0 = Stream.ReadUInt32();
			ambientLighting.UnknownData1 = Stream.ReadUInt32();
			ambientLighting.UnknownData2 = Stream.ReadUInt32();
			ambientLighting.UnknownData3 = Stream.ReadUInt32();
			ambientLighting.UnknownData4 = Stream.ReadUInt32();
			ambientLighting.UnknownData5 = Stream.ReadUInt32();
		}

		protected virtual void ReadNode(ref SourceNode sourceNode)
		{
			sourceNode.planenum = Stream.ReadInt32();
			sourceNode.front = Stream.ReadInt32();
			sourceNode.back = Stream.ReadInt32();
			ReadBBox(ref sourceNode.box);
			sourceNode.face_id = Stream.ReadUInt16();
			sourceNode.face_num = Stream.ReadUInt16();
			sourceNode.area = Stream.ReadInt16();
			sourceNode.paddding = Stream.ReadInt16();
		}

		protected void ReadBBox(ref SourceBoundingBox box)
		{
			box.boxMinX = Stream.ReadInt16();
			box.boxMinY = Stream.ReadInt16();
			box.boxMinZ = Stream.ReadInt16();
			box.boxMaxX = Stream.ReadInt16();
			box.boxMaxY = Stream.ReadInt16();
			box.boxMaxZ = Stream.ReadInt16();
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

		protected override void ReadModels()
		{
			this.SeekEntryAt(this.header.Models.offset);
			int size = this.EvalNumItems(this.header.Models.size, 12*4);
			this.models = new SourceModel[size];
			for (int i = 0; i < size; ++i)
			{
				ReadModel(ref this.models[i]);
			}
			this.AssertStreamPossition(this.header.Models.size + this.header.Models.offset);
		}

		private void ReadModel(ref SourceModel sourceModel)
		{
			float x, y, z;
			x = Stream.ReadSingle();
			y = Stream.ReadSingle();
			z = Stream.ReadSingle();
			sourceModel.mins = new Vector3(x,y,z);
			x = Stream.ReadSingle();
			y = Stream.ReadSingle();
			z = Stream.ReadSingle();
			sourceModel.maxs = new Vector3(x, y, z);
			x = Stream.ReadSingle();
			y = Stream.ReadSingle();
			z = Stream.ReadSingle();
			sourceModel.origin = new Vector3(x, y, z);
			sourceModel.headnode = Stream.ReadInt32();
			sourceModel.firstface = Stream.ReadInt32();
			sourceModel.numfaces = Stream.ReadInt32();
		}

		private void BuildFace(ref SourceFace face, VertexBufferSubmesh submesh, VertexBufferMesh streamMesh)
		{
			Vertex[] faceVertices = new Vertex[face.numedges];

			var plane = this.planes[face.planenum];
			var texture_id = this.texInfo[face.texinfo].texdata;

			Vector2 minUV0 = new Vector2(float.MaxValue, float.MaxValue);
			Vector2 minUV1 = new Vector2(float.MaxValue, float.MaxValue);
			Vector2 maxUV1 = new Vector2(float.MinValue, float.MinValue);
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

				Vertex vertex;
				this.BuildVertex(
					this.vertices[(short)edgesvertex0],
					(face.side == 0) ? plane.normal : -plane.normal,
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
					faceVertices[j].UV0 = new Vector3(0, 0, 0);
				}
			}

			int[] indices = new int[faceVertices.Length];
			for (int j = 0; j < faceVertices.Length; ++j)
			{
				indices[j] = streamMesh.VertexBuffer.Add(faceVertices[j]);
			}
			for (int j = 1; j < faceVertices.Length - 1; ++j)
			{
				submesh.Add(indices[0]);
				submesh.Add(indices[j]);
				submesh.Add(indices[j + 1]);
			}
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
						Vector3.Dot(surf.lm_vectorS, vector3) + surf.lm_distS - f.LightmapTextureMinsInLuxels[0],
						Vector3.Dot(surf.lm_vectorT, vector3) + surf.lm_distT - f.LightmapTextureMinsInLuxels[1],
						0.0f)
				};
			//if (f.LightmapTextureSizeInLuxels[0] == 0)
			res.UV1.X = (res.UV1.X + this.safeOffset) / (f.LightmapTextureSizeInLuxels[0] + 1.0f + this.safeBorderWidth);
			res.UV1.Y = (res.UV1.Y + this.safeOffset) / (f.LightmapTextureSizeInLuxels[1] + 1.0f + this.safeBorderWidth);

			SourceTexData tex = this.textures[surf.texdata];
			res.UV0 = new Vector3(
				res.UV0.X / ((tex.width != 0) ? tex.width : 256.0f), res.UV0.Y / ((tex.height != 0) ? tex.height : 256.0f), 0.0f);
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

		#region Implementation of IMaterialProvider

		public IMaterial CreateMaterial(BspMaterialKey material)
		{
			var baseFileName = this.GetMaterialFileName(material.Material);
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
			return sceneMaterial;
		}

		#endregion
	}
}