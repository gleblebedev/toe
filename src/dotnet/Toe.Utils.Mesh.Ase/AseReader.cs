using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;


using Toe.Utils.ToeMath;

namespace Toe.Utils.Mesh.Ase
{
	public class AseReader : ISceneReader
	{
		private readonly IStreamConverterFactory converterFactory;

		public AseReader(IStreamConverterFactory converterFactory)
		{
			this.converterFactory = converterFactory;
		}

		#region Constants and Fields

		private string basePath;

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Load mesh from stream.
		/// </summary>
		/// <param name="stream">Stream to read from.</param>
		/// <param name="basePath"> </param>
		/// <returns>Complete parsed mesh.</returns>
		public IScene Load(Stream stream, string basePath)
		{
			this.basePath = basePath ?? Directory.GetCurrentDirectory();
			using (var s = new StreamReader(stream))
			{
				var parser = new AseParser(s);
				return this.ParseScene(parser);
			}
		}

		#endregion

		#region Methods

	

		private static Color ParseColor(AseParser parser)
		{
			var r = (int)(255.0f * parser.ConsumeFloat());
			var g = (int)(255.0f * parser.ConsumeFloat());
			var b = (int)(255.0f * parser.ConsumeFloat());
			var fromArgb = Color.FromArgb(255, r, g, b);
			return fromArgb;
		}

		//private void BuildTangent(ref Vertex p0, ref Vertex p1, ref Vertex p2)
		//{
		//	var v1x = p1.Position.X - p0.Position.X;
		//	var v1y = p1.Position.Y - p0.Position.Y;
		//	var v1z = p1.Position.Z - p0.Position.Z;

		//	var v2x = p2.Position.X - p0.Position.X;
		//	var v2y = p2.Position.Y - p0.Position.Y;
		//	var v2z = p2.Position.Z - p0.Position.Z;

		//	var u1x = p1.UV0.X - p0.UV0.X;
		//	var u1y = p1.UV0.Y - p0.UV0.Y;

		//	var u2x = p2.UV0.X - p0.UV0.X;
		//	var u2y = p2.UV0.Y - p0.UV0.Y;

		//	var det = u1x * u2y - u2x * u1y;
		//	if (det == 0)
		//	{
		//		det = 1;
		//	}
		//	det = 1 / det;

		//	p0.Tangent =
		//		p1.Tangent =
		//		p2.Tangent =
		//		Float3.Normalize(
		//			new Float3((v1x * u2y - v2x * u1y) * det, (v1y * u2y - v2y * u1y) * det, (v1z * u2y - v2z * u1y) * det));
		//	p0.Binormal =
		//		p1.Binormal =
		//		p2.Binormal =
		//		Float3.Normalize(
		//			new Float3((-v1x * u2x + v2x * u1x) * det, (-v1y * u2x + v2y * u1x) * det, (-v1z * u2x + v2z * u1x) * det));
		//}

		private byte ClampCol(float a)
		{
			if (a <= 0)
			{
				return 0;
			}
			if (a >= 1)
			{
				return 255;
			}
			return (byte)(255.0f * a);
		}

		private void ParsMeshAnimation(AseParser parser, Scene scene, Node node)
		{
			parser.Consume("{");
			for (;;)
			{
				var attr = parser.Consume();
				if (attr == null || attr == "}")
				{
					break;
				}
				if (0 == string.Compare(attr, "*MESH", StringComparison.InvariantCultureIgnoreCase))
				{
					ParsSubMesh(parser, scene, node);
					continue;
				}
				parser.UnknownLexemError();
			}
		}

		private void ParsNormalList(AseParser parser, FaceNormal[] normals)
		{
			int faceIndex = 0;
			int faceVertexIndex = 0;
			parser.Consume("{");
			for (;;)
			{
				var attr = parser.Consume();
				if (attr == null || attr == "}")
				{
					break;
				}
				if (0 == string.Compare(attr, "*MESH_FACENORMAL", StringComparison.InvariantCultureIgnoreCase))
				{
					faceIndex = parser.ConsumeInt();
					var x = parser.ConsumeFloat();
					var y = parser.ConsumeFloat();
					var z = parser.ConsumeFloat();
					normals[faceIndex].A.Normal =
						normals[faceIndex].B.Normal = normals[faceIndex].C.Normal = normals[faceIndex].Normal = new Float3(x, y, z);
					faceVertexIndex = 0;
					continue;
				}
				if (0 == string.Compare(attr, "*MESH_VERTEXNORMAL", StringComparison.InvariantCultureIgnoreCase))
				{
					var index = parser.ConsumeInt();
					var x = parser.ConsumeFloat();
					var y = parser.ConsumeFloat();
					var z = parser.ConsumeFloat();
					Float3 v = new Float3(x, y, z);
					switch (faceVertexIndex)
					{
						case 0:
							normals[faceIndex].A.Index = index;
							normals[faceIndex].A.Normal = v;
							break;
						case 1:
							normals[faceIndex].B.Index = index;
							normals[faceIndex].B.Normal = v;
							break;
						case 2:
							normals[faceIndex].C.Index = index;
							normals[faceIndex].C.Normal = v;
							break;
					}
					++faceVertexIndex;
					continue;
				}
				parser.UnknownLexemError();
			}
		}
		private void ParseNode(AseParser parser, Scene scene, Action<AseParser,Scene,Node,string> extendedProperties = null)
		{
			var node = new Node();
			scene.Nodes.Add(node);
			parser.Consume("{");
			for (; ; )
			{
				var attr = parser.Consume();
				if (attr == null || attr == "}")
				{
					break;
				}
				if (0 == string.Compare(attr, "*NODE_NAME", StringComparison.InvariantCultureIgnoreCase))
				{
					node.Id = parser.Consume();
					continue;
				}
				if (0 == string.Compare(attr, "*NODE_PARENT", StringComparison.InvariantCultureIgnoreCase))
				{
					string parentName = parser.Consume();
					finishActions.Add((s) =>
					{
						var parentNode = s.FindNode(x => x.Id == parentName);
						if (parentNode != null)
						{
							s.Nodes.Remove(node);
							((INodeContainer)parentNode).Nodes.Add(node);
						}
						else
						{
							parentNode = parentNode;
						}
					});
					continue;
				}
				if (0 == string.Compare(attr, "*NODE_TM", StringComparison.InvariantCultureIgnoreCase))
				{
					this.SkipBlock(parser);
					continue;
				}
				if (extendedProperties != null)
				{
					extendedProperties(parser, scene, node,attr);
				}
				else
				{
					parser.UnknownLexemError();
				}
			}
		}
		private void ParseCamera(AseParser parser, Scene scene, Node node, string attr)
		{
			if (0 == string.Compare(attr, "*CAMERA_TYPE", StringComparison.InvariantCultureIgnoreCase))
			{
				switch (parser.Consume())
				{
					case "Target":
						break;
					default:
						break;
				}
				return;
			}
			if (0 == string.Compare(attr, "*CAMERA_SETTINGS", StringComparison.InvariantCultureIgnoreCase))
			{
				this.SkipBlock(parser);
				//*TIMEVALUE 0
				//*CAMERA_NEAR 0.0000
				//*CAMERA_FAR 1000.0000
				//*CAMERA_FOV 0.7854
				//*CAMERA_TDIST 140.6371

				return;
			}
		
			parser.UnknownLexemError();
		}
		private void ParseLight(AseParser parser, Scene scene, Node node, string attr)
		{
			if (0 == string.Compare(attr, "*LIGHT_TYPE", StringComparison.InvariantCultureIgnoreCase))
			{
				parser.Consume();
				return;
			}
			if (0 == string.Compare(attr, "*LIGHT_SHADOWS", StringComparison.InvariantCultureIgnoreCase))
			{
				parser.Consume();
				return;
			}
			if (0 == string.Compare(attr, "*LIGHT_USELIGHT", StringComparison.InvariantCultureIgnoreCase))
			{
				parser.ConsumeInt();
				return;
			}
			if (0 == string.Compare(attr, "*LIGHT_SPOTSHAPE", StringComparison.InvariantCultureIgnoreCase))
			{
				parser.Consume();
				return;
			}
			if (0 == string.Compare(attr, "*LIGHT_USEGLOBAL", StringComparison.InvariantCultureIgnoreCase))
			{
				parser.ConsumeInt();
				return;
			}
			if (0 == string.Compare(attr, "*LIGHT_ABSMAPBIAS", StringComparison.InvariantCultureIgnoreCase))
			{
				parser.ConsumeInt();
				return;
			}
			if (0 == string.Compare(attr, "*LIGHT_OVERSHOOT", StringComparison.InvariantCultureIgnoreCase))
			{
				parser.ConsumeInt();
				return;
			}
			if (0 == string.Compare(attr, "*LIGHT_SETTINGS", StringComparison.InvariantCultureIgnoreCase))
			{
				this.SkipBlock(parser);
				//*TIMEVALUE 0
				//*LIGHT_COLOR 1.0000	1.0000	1.0000
				//*LIGHT_INTENS 1.0000
				//*LIGHT_ASPECT -1.0000
				//*LIGHT_TDIST -1.0000
				//*LIGHT_MAPBIAS 1.0000
				//*LIGHT_MAPRANGE 4.0000
				//*LIGHT_MAPSIZE 512
				//*LIGHT_RAYBIAS 0.0000
				return;
			}

			parser.UnknownLexemError();
		}

		private void ParsSubMesh(AseParser parser, Scene scene, Node node,string attr)
		{
				
				if (0 == string.Compare(attr, "*MESH", StringComparison.InvariantCultureIgnoreCase))
				{
					ParsSubMesh(parser, scene, node);
					return;
				}
				if (0 == string.Compare(attr, "*MESH_ANIMATION", StringComparison.InvariantCultureIgnoreCase))
				{
					this.ParsMeshAnimation(parser, scene, node);
					return;
				}
				if (0 == string.Compare(attr, "*PROP_MOTIONBLUR", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeFloat();
					return;
				}
				if (0 == string.Compare(attr, "*PROP_CASTSHADOW", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeInt();
					return;
				}
				if (0 == string.Compare(attr, "*PROP_RECVSHADOW", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeInt();
					return;
				}
				if (0 == string.Compare(attr, "*MATERIAL_REF", StringComparison.InvariantCultureIgnoreCase))
				{
					var materialId = parser.ConsumeInt();
					if (scene.Materials != null && scene.Materials.Count > 0)
					{
						foreach (var submesh in node.Mesh.Submeshes)
						{
							submesh.Material = scene.Materials[materialId];
						}
					}
					return;
				}
				if (0 == string.Compare(attr, "*TM_ANIMATION", StringComparison.InvariantCultureIgnoreCase))
				{
					this.SkipBlock(parser);
					return;
				}
				if (0 == string.Compare(attr, "*WIREFRAME_COLOR", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeFloat();
					parser.ConsumeFloat();
					parser.ConsumeFloat();
					return;
				}

				parser.UnknownLexemError();
			
		}

		private void ParsSubMesh(AseParser parser, Scene scene, Node node)
		{
			var mesh = new SeparateStreamsMesh();
			var submesh = mesh.CreateSubmesh();
			node.Mesh = mesh;
			scene.Geometries.Add(mesh);

			ArrayMeshStream<Float3> vertices = null;
			ListMeshStream<Float3> normalStream = null;
			FaceNormal[] normals = null;
			ArrayMeshStream<Float3> tvertices = null;
			ArrayMeshStream<Color> cols = null;
			AseFace[] faces = null;
			AseTFace[] tfaces = null;
			Tuple<int, int, int>[] colFaces = null;
			//TODO: submesh can have it's own vertex streams
			parser.Consume("{");
			for (;;)
			{
				var attr = parser.Consume();
				if (attr == null || attr == "}")
				{
					break;
				}
				if (0 == string.Compare(attr, "*TIMEVALUE", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeFloat();
					continue;
				}
				if (0 == string.Compare(attr, "*MESH_NUMVERTEX", StringComparison.InvariantCultureIgnoreCase))
				{
					vertices = new ArrayMeshStream<Float3>(parser.ConsumeInt(), converterFactory);
					mesh.SetStream(Streams.Position, 0, vertices);
					continue;
				}
				if (0 == string.Compare(attr, "*MESH_VERTEX_LIST", StringComparison.InvariantCultureIgnoreCase))
				{
					this.ParseVertexList(parser, vertices);
					continue;
				}
				
				if (0 == string.Compare(attr, "*MESH_NUMTVERTEX", StringComparison.InvariantCultureIgnoreCase))
				{
					tvertices = new ArrayMeshStream<Float3>(parser.ConsumeInt(), converterFactory);
					mesh.SetStream(Streams.TexCoord, 0, tvertices);
					continue;
				}
				if (0 == string.Compare(attr, "*MESH_TVERTLIST", StringComparison.InvariantCultureIgnoreCase))
				{
					this.ParseTVertList(parser, tvertices);
					continue;
				}
				if (0 == string.Compare(attr, "*MESH_NUMTVFACES", StringComparison.InvariantCultureIgnoreCase))
				{
					tfaces = new AseTFace[parser.ConsumeInt()];
					continue;
				}
				if (0 == string.Compare(attr, "*MESH_TFACELIST", StringComparison.InvariantCultureIgnoreCase))
				{
					this.ParseTFaceList(parser, tfaces);
					continue;
				}
				if (0 == string.Compare(attr, "*MESH_NUMCVERTEX", StringComparison.InvariantCultureIgnoreCase))
				{
					cols = new ArrayMeshStream<Color>(parser.ConsumeInt(), converterFactory);
					mesh.SetStream(Streams.Color, 0, cols);
					continue;
				}
				if (0 == string.Compare(attr, "*MESH_CVERTLIST", StringComparison.InvariantCultureIgnoreCase))
				{
					this.ParseColList(parser, cols);
					continue;
				}
				if (0 == string.Compare(attr, "*MESH_NUMCVFACES", StringComparison.InvariantCultureIgnoreCase))
				{
					colFaces = new Tuple<int, int, int>[parser.ConsumeInt()];
					continue;
				}
				if (0 == string.Compare(attr, "*MESH_CFACELIST", StringComparison.InvariantCultureIgnoreCase))
				{
					this.ParseColFaceList(parser, colFaces);
					continue;
				}
				if (0 == string.Compare(attr, "*MESH_NORMALS", StringComparison.InvariantCultureIgnoreCase))
				{
					normals = new FaceNormal[faces.Length];
					this.ParsNormalList(parser, normals);
					continue;
				}
				if (0 == string.Compare(attr, "*MESH_NUMFACES", StringComparison.InvariantCultureIgnoreCase))
				{
					faces = new AseFace[parser.ConsumeInt()];
					continue;
				}

				if (0 == string.Compare(attr, "*MESH_FACE_LIST", StringComparison.InvariantCultureIgnoreCase))
				{
					this.ParseFaceList(parser, faces);
					continue;
				}
				parser.UnknownLexemError();
			}
			ListMeshStream<int> positionIndices = null;
			if (vertices != null)
			{
				positionIndices = new ListMeshStream<int>(faces.Length * 3,converterFactory);
				submesh.SetIndexStream(Streams.Position, 0, positionIndices);
			}
			ListMeshStream<int> normalIndices = null;
			if (normals != null)
			{
				normalStream = new ListMeshStream<Float3>(faces.Length * 3, converterFactory);
				mesh.SetStream(Streams.Normal, 0, normalStream);
				normalIndices = new ListMeshStream<int>(faces.Length * 3, converterFactory);
				submesh.SetIndexStream(Streams.Normal, 0, normalIndices);
			}
			ListMeshStream<int> colorIndices = null;
			if (cols != null)
			{
				colorIndices = new ListMeshStream<int>(faces.Length * 3, converterFactory);
				submesh.SetIndexStream(Streams.Color, 0, colorIndices);
			}
			ListMeshStream<int> texCoordIndices = null;
			if (tvertices != null)
			{
				texCoordIndices = new ListMeshStream<int>(faces.Length * 3, converterFactory);
				submesh.SetIndexStream(Streams.TexCoord, 0, texCoordIndices);
			}
			for (int i = 0; i < faces.Length; ++i)
			{
				// -------------------------------------------------- //
				if (positionIndices != null)
				{
					positionIndices.Add(faces[i].A);
				}
				if (normalIndices != null)
				{
					normalIndices.Add(normalStream.Count);
					normalStream.Add(normals[i].A.Normal);
				}
				if (colorIndices != null)
				{
					colorIndices.Add(colFaces[i].Item1);
				}
				if (texCoordIndices != null)
				{
					texCoordIndices.Add(tfaces[i].A);
				}
		
				// -------------------------------------------------- //
				if (positionIndices != null)
				{
					positionIndices.Add(faces[i].C);
				}
				if (normalIndices != null)
				{
					normalIndices.Add(normalStream.Count);
					normalStream.Add(normals[i].C.Normal);
				}
				if (colorIndices != null)
				{
					colorIndices.Add(colFaces[i].Item3);
				}
				if (texCoordIndices != null)
				{
					texCoordIndices.Add(tfaces[i].C);
				}
				// -------------------------------------------------- //
				if (positionIndices != null)
				{
					positionIndices.Add(faces[i].B);
				}
				if (normalIndices != null)
				{
					normalIndices.Add(normalStream.Count);
					normalStream.Add(normals[i].B.Normal);
				}
				if (colorIndices != null)
				{
					colorIndices.Add(colFaces[i].Item2);
				}
				if (texCoordIndices != null)
				{
					texCoordIndices.Add(tfaces[i].B);
				}
				// -------------------------------------------------- //
			}

		}

		private static Vertex BuildVertex(Float3[] vertices, int index0, FaceNormal[] normals,
		int faceIndex, int vertexAtFace, AseTFace[] tfaces, Color[] c, Float3[] tvertices, Tuple<int, int, int>[] colFaces, Float3 uv)
		{
			Vertex v = new Vertex { Position = vertices[index0] };
			if (normals != null)
			{
				v.Normal = normals[faceIndex].GetNormal(index0);
			}
			if (colFaces != null)
			{
				switch (vertexAtFace)
				{
					case 0:
						v.Color = c[colFaces[faceIndex].Item1];
						break;
					case 1:
						v.Color = c[colFaces[faceIndex].Item2];
						break;
					case 2:
						v.Color = c[colFaces[faceIndex].Item3];
						break;
				}
			}
			else
			{
				v.Color = Color.FromArgb(255, 255, 255, 255);
			}
			v.UV1 = v.UV0 = new Float3(uv.X, 1.0f - uv.Y, uv.Z);
			return v;
		}

		private void ParseColFaceList(AseParser parser, Tuple<int, int, int>[] colFaces)
		{
			parser.Consume("{");
			for (; ; )
			{
				var attr = parser.Consume();
				if (attr == null || attr == "}")
				{
					break;
				}
				if (0 == string.Compare(attr, "*MESH_CFACE", StringComparison.InvariantCultureIgnoreCase))
				{
					var index = parser.ConsumeInt();
					var a = parser.ConsumeInt();
					var b = parser.ConsumeInt();
					var c = parser.ConsumeInt();
					colFaces[index] = new Tuple<int, int, int>(a,b,c);
					continue;
				}
				parser.UnknownLexemError();
			}
		}

		private void Parse3DsMaxSciiExport(AseParser parser, Scene scene)
		{
			parser.ConsumeInt();
		}

		private void ParseColList(AseParser parser, IList<Color> colors)
		{
			parser.Consume("{");
			for (;;)
			{
				var attr = parser.Consume();
				if (attr == null || attr == "}")
				{
					break;
				}
				if (0 == string.Compare(attr, "*MESH_VERTCOL", StringComparison.InvariantCultureIgnoreCase))
				{
					var index = parser.ConsumeInt();
					var a = parser.ConsumeFloat();
					var b = parser.ConsumeFloat();
					var c = parser.ConsumeFloat();
					colors[index] = Color.FromArgb(255, this.ClampCol(a), this.ClampCol(b), this.ClampCol(c));
					continue;
				}
				parser.UnknownLexemError();
			}
		}

		private void ParseComment(AseParser parser, Scene scene)
		{
			parser.Consume();
		}

		private void ParseFaceList(AseParser parser, IList<AseFace> faces)
		{
			parser.Consume("{");
			for (;;)
			{
				var attr = parser.Consume();
				if (attr == null || attr == "}")
				{
					break;
				}
				if (0 == string.Compare(attr, "*MESH_FACE", StringComparison.InvariantCultureIgnoreCase))
				{
					var index = int.Parse(parser.Consume().TrimEnd(':'), CultureInfo.InvariantCulture);
					parser.Consume("A:");
					var a = parser.ConsumeInt();
					parser.Consume("B:");
					var b = parser.ConsumeInt();
					parser.Consume("C:");
					var c = parser.ConsumeInt();

					parser.Consume("AB:");
					var ab = parser.ConsumeInt();
					parser.Consume("BC:");
					var bc = parser.ConsumeInt();
					parser.Consume("CA:");
					var ca = parser.ConsumeInt();

					faces[index] = new AseFace { A = a, B = b, C = c };

					if (parser.Lexem == "*MESH_SMOOTHING")
					{
						parser.Consume();
						if (parser.Lexem != "*MESH_MTLID")
						{
							parser.ConsumeInt();
						}
					}
					if (parser.Lexem == "*MESH_MTLID")
					{
						parser.Consume();
						parser.ConsumeInt();
					}
					continue;
				}
				parser.UnknownLexemError();
			}
		}

		private IColorSource ParseMap(AseParser parser, Scene scene)
		{
			var image = new FileReferenceImage();
			var texture = new ImageColorSource { Image = image };
			parser.Consume("{");
			for (;;)
			{
				var attr = parser.Consume();
				if (attr == null || attr == "}")
				{
					break;
				}
				if (0 == string.Compare(attr, "*MAP_NAME", StringComparison.InvariantCultureIgnoreCase))
				{
					image.Id = image.Name = parser.Consume();
					continue;
				}
				if (0 == string.Compare(attr, "*MAP_CLASS", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.Consume();
					continue;
				}
				if (0 == string.Compare(attr, "*MAP_SUBNO", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.Consume();
					continue;
				}
				if (0 == string.Compare(attr, "*MAP_AMOUNT", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeFloat();
					continue;
				}
				if (0 == string.Compare(attr, "*BITMAP", StringComparison.InvariantCultureIgnoreCase))
				{
					image.Path = Path.Combine(this.basePath, parser.Consume());
					continue;
				}
				if (0 == string.Compare(attr, "*MAP_TYPE", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.Consume();
					continue;
				}
				if (0 == string.Compare(attr, "*UVW_U_OFFSET", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeFloat();
					continue;
				}
				if (0 == string.Compare(attr, "*UVW_V_OFFSET", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeFloat();
					continue;
				}
				if (0 == string.Compare(attr, "*UVW_U_TILING", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeFloat();
					continue;
				}
				if (0 == string.Compare(attr, "*UVW_V_TILING", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeFloat();
					continue;
				}
				if (0 == string.Compare(attr, "*UVW_ANGLE", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeFloat();
					continue;
				}
				if (0 == string.Compare(attr, "*UVW_BLUR", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeFloat();
					continue;
				}
				if (0 == string.Compare(attr, "*UVW_BLUR_OFFSET", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeFloat();
					continue;
				}
				if (0 == string.Compare(attr, "*UVW_NOUSE_AMT", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeFloat();
					continue;
				}
				if (0 == string.Compare(attr, "*UVW_NOISE_SIZE", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeFloat();
					continue;
				}
				if (0 == string.Compare(attr, "*UVW_NOISE_LEVEL", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeFloat();
					continue;
				}
				if (0 == string.Compare(attr, "*UVW_NOISE_PHASE", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeFloat();
					continue;
				}
				if (0 == string.Compare(attr, "*BITMAP_FILTER", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.Consume();
					continue;
				}
				parser.UnknownLexemError();
			}
			return texture;
		}

		private IMaterial ParseMaterial(AseParser parser, Scene scene)
		{
			var sceneEffect = new SceneEffect() {CullMode = CullMode.Front};
			var m = new SceneMaterial { Effect = sceneEffect };
			parser.Consume("{");
			for (;;)
			{
				var attr = parser.Consume();
				if (attr == null || attr == "}")
				{
					break;
				}
				if (0 == string.Compare(attr, "*MATERIAL_NAME", StringComparison.InvariantCultureIgnoreCase))
				{
					m.Id = m.Name = sceneEffect.Id = sceneEffect.Name = parser.Consume();
					continue;
				}
				if (0 == string.Compare(attr, "*MATERIAL_CLASS", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.Consume();
					continue;
				}
				if (0 == string.Compare(attr, "*MATERIAL_AMBIENT", StringComparison.InvariantCultureIgnoreCase))
				{
					sceneEffect.Ambient = new SolidColorSource { Color = ParseColor(parser) };
					continue;
				}
				if (0 == string.Compare(attr, "*MATERIAL_DIFFUSE", StringComparison.InvariantCultureIgnoreCase))
				{
					sceneEffect.Diffuse = new SolidColorSource { Color = ParseColor(parser) };
					continue;
				}
				if (0 == string.Compare(attr, "*MATERIAL_SPECULAR", StringComparison.InvariantCultureIgnoreCase))
				{
					sceneEffect.Specular = new SolidColorSource { Color = ParseColor(parser) };
					continue;
				}
				if (0 == string.Compare(attr, "*MATERIAL_SHINE", StringComparison.InvariantCultureIgnoreCase))
				{
					sceneEffect.Shininess = parser.ConsumeFloat();
					continue;
				}
				if (0 == string.Compare(attr, "*MATERIAL_SHINESTRENGTH", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeFloat();
					continue;
				}
				if (0 == string.Compare(attr, "*MATERIAL_TRANSPARENCY", StringComparison.InvariantCultureIgnoreCase))
				{
					sceneEffect.Transparency = parser.ConsumeFloat();
					continue;
				}
				if (0 == string.Compare(attr, "*MATERIAL_WIRESIZE", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeFloat();
					continue;
				}
				if (0 == string.Compare(attr, "*MATERIAL_XP_FALLOFF", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeFloat();
					continue;
				}
				if (0 == string.Compare(attr, "*MATERIAL_SELFILLUM", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeFloat();
					continue;
				}
				if (0 == string.Compare(attr, "*MATERIAL_SHADING", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.Consume();
					continue;
				}
				if (0 == string.Compare(attr, "*MATERIAL_FALLOFF", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.Consume();
					continue;
				}
				if (0 == string.Compare(attr, "*MATERIAL_XP_TYPE", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.Consume();
					continue;
				}

				if (0 == string.Compare(attr, "*MAP_AMBIENT", StringComparison.InvariantCultureIgnoreCase))
				{
					sceneEffect.Ambient = this.ParseMap(parser, scene);
					continue;
				}
				if (0 == string.Compare(attr, "*MAP_DIFFUSE", StringComparison.InvariantCultureIgnoreCase))
				{
					sceneEffect.Diffuse = this.ParseMap(parser, scene);
					continue;
				}
				if (0 == string.Compare(attr, "*NUMSUBMTLS", StringComparison.InvariantCultureIgnoreCase))
				{
					var numMaterials = parser.ConsumeInt();
					continue;
				}
				if (0 == string.Compare(attr, "*SUBMATERIAL", StringComparison.InvariantCultureIgnoreCase))
				{
					var index = parser.ConsumeInt();
					while (m.Submaterials.Count <= index) m.Submaterials.Add(null);
					var subm = ParseMaterial(parser, scene);
					m.Submaterials[index] = subm;
					continue;
				}
				

				parser.UnknownLexemError();
			}
			return m;
		}

		private void ParseMaterials(AseParser parser, Scene scene)
		{
			parser.Consume("{");
			for (;;)
			{
				var attr = parser.Consume();
				if (attr == null || attr == "}")
				{
					break;
				}
				if (0 == string.Compare(attr, "*MATERIAL_COUNT", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeInt();
					continue;
				}
				if (0 == string.Compare(attr, "*MATERIAL", StringComparison.InvariantCultureIgnoreCase))
				{
					var id = parser.ConsumeInt();
					while (scene.Materials.Count <= id)
					{
						scene.Materials.Add(null);
					}
					scene.Materials[id] = this.ParseMaterial(parser, scene);
					continue;
				}
				parser.UnknownLexemError();
			}
		}
		List<Action<Scene>> finishActions = new List<Action<Scene>>();
		private IScene ParseScene(AseParser parser)
		{
			Scene s = new Scene();
			for (;;)
			{
				var meshSection = parser.Consume();
				if (meshSection == null)
				{
					
					break;
				}
				if (0 == string.Compare(meshSection, "*3DSMAX_ASCIIEXPORT", StringComparison.InvariantCultureIgnoreCase))
				{
					this.Parse3DsMaxSciiExport(parser, s);
					continue;
				}
				if (0 == string.Compare(meshSection, "*COMMENT", StringComparison.InvariantCultureIgnoreCase))
				{
					this.ParseComment(parser, s);
					continue;
				}
				if (0 == string.Compare(meshSection, "*SCENE", StringComparison.InvariantCultureIgnoreCase))
				{
					this.ParseScene(parser, s);
					continue;
				}
				if (0 == string.Compare(meshSection, "*MATERIAL_LIST", StringComparison.InvariantCultureIgnoreCase))
				{
					this.ParseMaterials(parser, s);
					continue;
				}
				if (0 == string.Compare(meshSection, "*CAMERAOBJECT", StringComparison.InvariantCultureIgnoreCase))
				{
					this.ParseNode(parser, s, ParseCamera);
					continue;
				}
				if (0 == string.Compare(meshSection, "*GEOMOBJECT", StringComparison.InvariantCultureIgnoreCase))
				{
					ParseNode(parser, s, ParsSubMesh);
					continue;
				}
				if (0 == string.Compare(meshSection, "*LIGHTOBJECT", StringComparison.InvariantCultureIgnoreCase))
				{
					ParseNode(parser, s, ParseLight);
					continue;
				}
				parser.UnknownLexemError();
			}
			foreach (var finishAction in finishActions)
			{
				finishAction(s);
			}
			finishActions.Clear();
			return s;
		}

	
		private void ParseScene(AseParser parser, Scene scene)
		{
			parser.Consume("{");
			for (;;)
			{
				var attr = parser.Consume();
				if (attr == null || attr == "}")
				{
					break;
				}
				if (0 == string.Compare(attr, "*SCENE_FILENAME", StringComparison.InvariantCultureIgnoreCase))
				{
					scene.Id = parser.Consume();
					continue;
				}
				if (0 == string.Compare(attr, "*SCENE_FIRSTFRAME", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeInt();
					continue;
				}
				if (0 == string.Compare(attr, "*SCENE_LASTFRAME", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeInt();
					continue;
				}
				if (0 == string.Compare(attr, "*SCENE_FRAMESPEED", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeFloat();
					continue;
				}
				if (0 == string.Compare(attr, "*SCENE_TICKSPERFRAME", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeFloat();
					continue;
				}
				if (0 == string.Compare(attr, "*SCENE_BACKGROUND_STATIC", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeFloat();
					parser.ConsumeFloat();
					parser.ConsumeFloat();
					continue;
				}
				if (0 == string.Compare(attr, "*SCENE_AMBIENT_STATIC", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeFloat();
					parser.ConsumeFloat();
					parser.ConsumeFloat();
					continue;
				}
				parser.UnknownLexemError();
			}
		}

		private void ParseTFaceList(AseParser parser, IList<AseTFace> faces)
		{
			parser.Consume("{");
			for (;;)
			{
				var attr = parser.Consume();
				if (attr == null || attr == "}")
				{
					break;
				}
				if (0 == string.Compare(attr, "*MESH_TFACE", StringComparison.InvariantCultureIgnoreCase))
				{
					var index = parser.ConsumeInt();
					var a = parser.ConsumeInt();
					var b = parser.ConsumeInt();
					var c = parser.ConsumeInt();
					faces[index] = new AseTFace { A = a, B = b, C = c };
					continue;
				}
				parser.UnknownLexemError();
			}
		}

		private void ParseTVertList(AseParser parser, IList<Float3> vertices)
		{
			parser.Consume("{");
			for (;;)
			{
				var attr = parser.Consume();
				if (attr == null || attr == "}")
				{
					break;
				}
				if (0 == string.Compare(attr, "*MESH_TVERT", StringComparison.InvariantCultureIgnoreCase))
				{
					var index = parser.ConsumeInt();
					var x = parser.ConsumeFloat();
					var y = 1.0f - parser.ConsumeFloat();
					var z = parser.ConsumeFloat();

					vertices[index] = new Float3(x, y, z);
					continue;
				}
				parser.UnknownLexemError();
			}
		}

		private void ParseVertexList(AseParser parser, IList<Float3> vertices)
		{
			parser.Consume("{");
			for (;;)
			{
				var attr = parser.Consume();
				if (attr == null || attr == "}")
				{
					break;
				}
				if (0 == string.Compare(attr, "*MESH_VERTEX", StringComparison.InvariantCultureIgnoreCase))
				{
					var index = parser.ConsumeInt();
					var x = parser.ConsumeFloat();
					var y = parser.ConsumeFloat();
					var z = parser.ConsumeFloat();
					vertices[index] = new Float3(x, y, z);
					continue;
				}
				parser.UnknownLexemError();
			}
		}

		private void SkipBlock(AseParser parser)
		{
			parser.Consume("{");
			int counter = 1;
			for (;;)
			{
				var attr = parser.Consume();
				if (attr == null)
				{
					break;
				}
				if (attr == "}")
				{
					--counter;
					if (counter == 0)
					{
						break;
					}
					continue;
				}
				if (attr == "{")
				{
					++counter;
					continue;
				}
			}
		}

		#endregion

		private struct FaceNormal
		{
			#region Constants and Fields

			internal FaceVertexNormal A;

			internal FaceVertexNormal B;

			internal FaceVertexNormal C;

			internal Float3 Normal;

			#endregion

			#region Public Methods and Operators

			public Float3 GetNormal(int index0)
			{
				if (index0 == this.A.Index)
				{
					return this.A.Normal;
				}
				if (index0 == this.B.Index)
				{
					return this.B.Normal;
				}
				if (index0 == this.C.Index)
				{
					return this.C.Normal;
				}
				return this.Normal;
			}

			#endregion
		}

		private struct FaceVertexNormal
		{
			#region Constants and Fields

			internal int Index;

			internal Float3 Normal;

			#endregion
		}
	}
}