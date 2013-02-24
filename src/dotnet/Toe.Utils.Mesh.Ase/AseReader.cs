using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;

using OpenTK;

namespace Toe.Utils.Mesh.Ase
{
	public class AseReader : ISceneReader
	{
		private string basePath;

		#region Implementation of IMeshReader

		/// <summary>
		/// Load mesh from stream.
		/// </summary>
		/// <param name="stream">Stream to read from.</param>
		/// <param name="basePath"> </param>
		/// <returns>Complete parsed mesh.</returns>
		public IScene Load(Stream stream, string basePath)
		{
			this.basePath = basePath?? Directory.GetCurrentDirectory();
			using (var s = new StreamReader(stream))
			{
				var parser = new AseParser(s);
				return this.ParseScene(parser);
			}
		}

		private IScene ParseScene(AseParser parser)
		{
			Scene s = new Scene();
			for (;;)
			{
				var meshSection = parser.Consume();
				if (meshSection == null) return s;
				if (0 == string.Compare(meshSection, "*3DSMAX_ASCIIEXPORT", StringComparison.InvariantCultureIgnoreCase))
				{
					Parse3DsMaxSciiExport(parser, s);
					continue;
				}
				if (0==string.Compare(meshSection, "*COMMENT", StringComparison.InvariantCultureIgnoreCase))
				{
					ParseComment(parser, s);
					continue;
				}
				if (0==string.Compare(meshSection, "*SCENE", StringComparison.InvariantCultureIgnoreCase))
				{
					ParseScene(parser, s);
					continue;
				}
				if (0 == string.Compare(meshSection, "*MATERIAL_LIST", StringComparison.InvariantCultureIgnoreCase))
				{
					ParseMaterials(parser, s);
					continue;
				}
				if (0 == string.Compare(meshSection, "*CAMERAOBJECT", StringComparison.InvariantCultureIgnoreCase))
				{
					this.SkipBlock(parser);
					continue;
				}
				if (0 == string.Compare(meshSection, "*GEOMOBJECT", StringComparison.InvariantCultureIgnoreCase))
				{
					ParsSubMesh(parser, s);
					continue;
				}
				parser.UnknownLexemError();
			}
		}

		private void ParseMaterials(AseParser parser, Scene scene)
		{
			parser.Consume("{");
			for (; ; )
			{
				var attr = parser.Consume();
				if (attr == null || attr == "}") break;
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
					scene.Materials[id] = ParseMaterial(parser, scene);
					continue;
				}
				parser.UnknownLexemError();
			}
		}

		private IMaterial ParseMaterial(AseParser parser, Scene scene)
		{
			var sceneEffect = new SceneEffect();
			var m = new SceneMaterial() { Effect = sceneEffect };
			parser.Consume("{");
			for (; ; )
			{
				var attr = parser.Consume();
				if (attr == null || attr == "}") break;
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
					sceneEffect.Ambient = new SolidColorSource(){Color = ParseColor(parser)};
					continue;
				}
				if (0 == string.Compare(attr, "*MATERIAL_DIFFUSE", StringComparison.InvariantCultureIgnoreCase))
				{
					sceneEffect.Diffuse = new SolidColorSource() { Color = ParseColor(parser) };
					continue;
				}
				if (0 == string.Compare(attr, "*MATERIAL_SPECULAR", StringComparison.InvariantCultureIgnoreCase))
				{
					sceneEffect.Specular = new SolidColorSource() { Color = ParseColor(parser) };
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
				if (0 == string.Compare(attr, "*MAP_DIFFUSE", StringComparison.InvariantCultureIgnoreCase))
				{
					sceneEffect.Diffuse = ParseMap(parser, scene);
					continue;
				}
				
				parser.UnknownLexemError();
			}
			return m;
		}

		private IColorSource ParseMap(AseParser parser, Scene scene)
		{
			var image = new FileReferenceImage();
			var texture = new ImageColorSource { Image = image };
			parser.Consume("{");
			for (; ; )
			{
				var attr = parser.Consume();
				if (attr == null || attr == "}") break;
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
					image.Path = Path.Combine(basePath, parser.Consume());
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

		private static Color ParseColor(AseParser parser)
		{
			var r = (int)(255.0f * parser.ConsumeFloat());
			var g = (int)(255.0f * parser.ConsumeFloat());
			var b = (int)(255.0f * parser.ConsumeFloat());
			var fromArgb = Color.FromArgb(255, r, g, b);
			return fromArgb;
		}

		private void ParsSubMesh(AseParser parser, Scene scene)
		{
			var node = new Node();
			scene.Nodes.Add(node);
			parser.Consume("{");
			for (; ; )
			{
				var attr = parser.Consume();
				if (attr == null || attr == "}")
					break;
				if (0 == string.Compare(attr, "*NODE_NAME", StringComparison.InvariantCultureIgnoreCase))
				{
					node.Id = parser.Consume();
					continue;
				}
				if (0 == string.Compare(attr, "*NODE_TM", StringComparison.InvariantCultureIgnoreCase))
				{
					this.SkipBlock(parser);
					continue;
				}
				if (0 == string.Compare(attr, "*MESH", StringComparison.InvariantCultureIgnoreCase))
				{
					ParsSubMesh(parser, node);
					continue;
				}
				if (0 == string.Compare(attr, "*MESH_ANIMATION", StringComparison.InvariantCultureIgnoreCase))
				{
					ParsMeshAnimation(parser, node);
					continue;
				}
				if (0 == string.Compare(attr, "*PROP_MOTIONBLUR", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeFloat();
					continue;
				}
				if (0 == string.Compare(attr, "*PROP_CASTSHADOW", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeInt();
					continue;
				}
				if (0 == string.Compare(attr, "*PROP_RECVSHADOW", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeInt();
					continue;
				}
				if (0 == string.Compare(attr, "*MATERIAL_REF", StringComparison.InvariantCultureIgnoreCase))
				{
					var materialId = parser.ConsumeInt();
					if (scene.Materials!= null && scene.Materials.Count > 0)
					{
						foreach (var submesh in node.Mesh.Submeshes)
						{
							submesh.Material = scene.Materials[materialId];
						}
					}
					continue;
				}
				if (0 == string.Compare(attr, "*TM_ANIMATION", StringComparison.InvariantCultureIgnoreCase))
				{
					this.SkipBlock(parser);
					continue;
				}
				if (0 == string.Compare(attr, "*WIREFRAME_COLOR", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeFloat();
					parser.ConsumeFloat();
					parser.ConsumeFloat();
					continue;
				}
				
				parser.UnknownLexemError();
			}
		}

		private void ParsMeshAnimation(AseParser parser, Node node)
		{
			parser.Consume("{");
			for (; ; )
			{
				var attr = parser.Consume();
				if (attr == null || attr == "}")
					break;
				if (0 == string.Compare(attr, "*MESH", StringComparison.InvariantCultureIgnoreCase))
				{
					ParsSubMesh(parser, node);
					continue;
				}
				parser.UnknownLexemError();
			}
		}

		struct FaceVertexNormal
		{
			internal int Index;
			internal Vector3 Normal;
		}
		struct FaceNormal
		{
			internal Vector3 Normal;

			internal FaceVertexNormal A;
			internal FaceVertexNormal B;
			internal FaceVertexNormal C;

			public Vector3 GetNormal(int index0)
			{
				if (index0 == A.Index) return A.Normal;
				if (index0 == B.Index) return B.Normal;
				if (index0 == C.Index) return C.Normal;
				return Normal;
			}
		}
		private void ParsSubMesh(AseParser parser, Node node)
		{
			var mesh = new VertexBufferMesh();
			var submesh = (VertexBufferSubmesh)mesh.CreateSubmesh();
			node.Mesh = mesh;

			Vector3[] vertices = null;
			FaceNormal[] normals = null;
			Vector3[] tvertices = null;
			Color[] cols = null;
			AseFace[] faces = null;
			AseTFace[] tfaces = null;
			//TODO: submesh can have it's own vertex streams
			parser.Consume("{");
			for (; ; )
			{
				var attr = parser.Consume();
				if (attr == null || attr == "}")
					break;
				if (0 == string.Compare(attr, "*TIMEVALUE", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeFloat();
					continue;
				}
				if (0 == string.Compare(attr, "*MESH_NUMVERTEX", StringComparison.InvariantCultureIgnoreCase))
				{
					vertices = new Vector3[parser.ConsumeInt()];
					continue;
				}
				if (0 == string.Compare(attr, "*MESH_VERTEX_LIST", StringComparison.InvariantCultureIgnoreCase))
				{
					this.ParseVertexList(parser, vertices);
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
				if (0 == string.Compare(attr, "*MESH_NUMTVERTEX", StringComparison.InvariantCultureIgnoreCase))
				{
					tvertices = new Vector3[parser.ConsumeInt()];
					continue;
				}
				if (0 == string.Compare(attr, "*MESH_TVERTLIST", StringComparison.InvariantCultureIgnoreCase))
				{
					ParseTVertList(parser, tvertices);
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
					cols = new Color[parser.ConsumeInt()];
					continue;
				}
				if (0 == string.Compare(attr, "*MESH_CVERTLIST", StringComparison.InvariantCultureIgnoreCase))
				{
					this.ParseColList(parser, cols);
					continue;
				}
				if (0 == string.Compare(attr, "*MESH_NUMCVFACES", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeInt();
					continue;
				}
				if (0 == string.Compare(attr, "*MESH_CFACELIST", StringComparison.InvariantCultureIgnoreCase))
				{
					this.SkipBlock(parser);
					continue;
				}
				if (0 == string.Compare(attr, "*MESH_NORMALS", StringComparison.InvariantCultureIgnoreCase))
				{
					normals = new FaceNormal[faces.Length];
					this.ParsNormalList(parser,normals);
					continue;
				}
				parser.UnknownLexemError();
			}
			int i = 0;
			foreach (var aseFace in faces)
			{
				Vertex c = BuildVertex(vertices, aseFace.C, normals, i, tfaces, cols, tvertices, (tfaces != null && tvertices != null) ? tvertices[tfaces[i].C] : Vector3.Zero);
				Vertex b = BuildVertex(vertices, aseFace.B, normals, i, tfaces, cols, tvertices, (tfaces != null && tvertices != null) ? tvertices[tfaces[i].B] : Vector3.Zero);
				Vertex a = BuildVertex(vertices, aseFace.A, normals, i, tfaces, cols, tvertices, (tfaces != null && tvertices != null) ? tvertices[tfaces[i].A] : Vector3.Zero);
				BuildTangent(ref a, ref b, ref c);
				submesh.Add(mesh.VertexBuffer.Add(a));
				submesh.Add(mesh.VertexBuffer.Add(b));
				submesh.Add(mesh.VertexBuffer.Add(c));
				++i;
			}
			mesh.IsBinormalStreamAvailable = true;
			mesh.IsTangentStreamAvailable = true;
			if (cols != null) mesh.IsColorStreamAvailable = true;
			if (normals != null) mesh.IsNormalStreamAvailable = true;
			if (tfaces != null) mesh.IsUV0StreamAvailable = true;
		}

		private void BuildTangent(ref Vertex p0, ref Vertex p1, ref Vertex p2)
		{
			var v1x = p1.Position.X - p0.Position.X;
			var v1y = p1.Position.Y - p0.Position.Y;
			var v1z = p1.Position.Z - p0.Position.Z;

			var v2x = p2.Position.X - p0.Position.X;
			var v2y = p2.Position.Y - p0.Position.Y;
			var v2z = p2.Position.Z - p0.Position.Z;

			var u1x = p1.UV0.X - p0.UV0.X;
			var u1y = p1.UV0.Y - p0.UV0.Y;

			var u2x = p2.UV0.X - p0.UV0.X;
			var u2y = p2.UV0.Y - p0.UV0.Y;

			var det = u1x * u2y - u2x * u1y;
			if (det == 0) det = 1;
			det = 1 / det;

			p0.Tangent = p1.Tangent = p2.Tangent = Vector3.Normalize(new Vector3((v1x * u2y - v2x * u1y) * det, (v1y * u2y - v2y * u1y) * det, (v1z * u2y - v2z * u1y) * det));
			p0.Binormal = p1.Binormal = p2.Binormal = Vector3.Normalize(new Vector3((-v1x * u2x + v2x * u1x) * det, (-v1y * u2x + v2y * u1x) * det, (-v1z * u2x + v2z * u1x) * det));
		}

		private static Vertex BuildVertex(
			Vector3[] vertices, int index0, FaceNormal[] normals, int faceIndex, AseTFace[] tfaces, Color[] c, Vector3[] tvertices, Vector3 uv)
		{
			Vertex v = new Vertex { Position = vertices[index0] };
			if (normals != null)
			{
				v.Normal = normals[faceIndex].GetNormal(index0);
			}
			v.Color = c != null && c.Length > 0 ? c[index0] : Color.FromArgb(255,255,255,255);
			v.UV1 = v.UV0 = new Vector3(uv.X, 1.0f - uv.Y, uv.Z);
			return v;
		}

		
		private void ParseColList(AseParser parser, IList<Color> colors)
		{
			parser.Consume("{");
			for (; ; )
			{
				var attr = parser.Consume();
				if (attr == null || attr == "}") break;
				if (0 == string.Compare(attr, "*MESH_VERTCOL", StringComparison.InvariantCultureIgnoreCase))
				{
					var index = parser.ConsumeInt();
					var a = parser.ConsumeFloat();
					var b = parser.ConsumeFloat();
					var c = parser.ConsumeFloat();
					colors[index] = Color.FromArgb(255, ClampCol(255.0f * a), ClampCol(255.0f * b), ClampCol(255.0f * c));
					continue;
				}
				parser.UnknownLexemError();
			}
		}

		private byte ClampCol(float a)
		{
			if (a <= 0) return 0;
			if (a >= 1) return 255;
			return (byte)(255.0f * a);
		}

		private void ParseTFaceList(AseParser parser, IList<AseTFace> faces)
		{
			parser.Consume("{");
			for (; ; )
			{
				var attr = parser.Consume();
				if (attr == null || attr == "}") break;
				if (0 == string.Compare(attr, "*MESH_TFACE", StringComparison.InvariantCultureIgnoreCase))
				{
					var index = parser.ConsumeInt();
					var a = parser.ConsumeInt();
					var b = parser.ConsumeInt();
					var c = parser.ConsumeInt();
					faces[index] = new AseTFace(){A=a,B=b,C=c};
					continue;
				}
				parser.UnknownLexemError();
			}
		}

		private void ParseFaceList(AseParser parser, IList<AseFace> faces)
		{
			parser.Consume("{");
			for (; ; )
			{
				var attr = parser.Consume();
				if (attr == null || attr == "}")
					break;
				if (0 == string.Compare(attr, "*MESH_FACE", StringComparison.InvariantCultureIgnoreCase))
				{
					var index = int.Parse(parser.Consume().TrimEnd(':'),CultureInfo.InvariantCulture);
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

					faces[index] = new AseFace(){A=a,B=b,C=c};

					if (parser.Lexem == "*MESH_SMOOTHING")
					{
						parser.Consume();
						if (parser.Lexem != "*MESH_MTLID")
							parser.ConsumeInt();
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

		private void ParseTVertList(AseParser parser, IList<Vector3> vertices)
		{
			parser.Consume("{");
			for (; ; )
			{
				var attr = parser.Consume();
				if (attr == null || attr == "}")
					break;
				if (0 == string.Compare(attr, "*MESH_TVERT", StringComparison.InvariantCultureIgnoreCase))
				{
					var index = parser.ConsumeInt();
					var x = parser.ConsumeFloat();
					var y = parser.ConsumeFloat();
					var z = parser.ConsumeFloat();
					
					vertices[index] = new Vector3(x, y,z);
					continue;
				}
				parser.UnknownLexemError();
			}
		}

		private void ParseVertexList(AseParser parser, Vector3[] vertices)
		{
			parser.Consume("{");
			for (; ; )
			{
				var attr = parser.Consume();
				if (attr == null || attr == "}")
					break;
				if (0 == string.Compare(attr, "*MESH_VERTEX", StringComparison.InvariantCultureIgnoreCase))
				{
					var index = parser.ConsumeInt();
					var x = parser.ConsumeFloat();
					var y = parser.ConsumeFloat();
					var z = parser.ConsumeFloat();
					vertices[index] = new Vector3(x, y, z);
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
			for (; ; )
			{
				var attr = parser.Consume();
				if (attr == null || attr == "}")
					break;
				if (0 == string.Compare(attr, "*MESH_FACENORMAL", StringComparison.InvariantCultureIgnoreCase))
				{
					faceIndex = parser.ConsumeInt();
					var x = parser.ConsumeFloat();
					var y = parser.ConsumeFloat();
					var z = parser.ConsumeFloat();
					normals[faceIndex].A.Normal = normals[faceIndex].B.Normal = normals[faceIndex].C.Normal = normals[faceIndex].Normal = new Vector3(x, y, z);
					faceVertexIndex = 0;
					continue;
				}
				if (0 == string.Compare(attr, "*MESH_VERTEXNORMAL", StringComparison.InvariantCultureIgnoreCase))
				{
					var index = parser.ConsumeInt();
					var x = parser.ConsumeFloat();
					var y = parser.ConsumeFloat();
					var z = parser.ConsumeFloat();
					Vector3 v = new Vector3(x,y,z);
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

		private void SkipBlock(AseParser parser)
		{
			parser.Consume("{");
			int counter = 1;
			for (; ; )
			{
				var attr = parser.Consume();
				if (attr == null)
					break;
				if (attr == "}")
				{
					--counter;
					if (counter == 0)
						break;
					continue;
				}
				if (attr == "{")
				{
					++counter;
					continue;
				}
			}
		}

		private void ParseScene(AseParser parser, Scene scene)
		{
			parser.Consume("{");
			for (; ; )
			{
				var attr = parser.Consume();
				if (attr==null || attr == "}")
					break;
				if (0==string.Compare(attr, "*SCENE_FILENAME", StringComparison.InvariantCultureIgnoreCase))
				{
					scene.Id = parser.Consume();
					continue;
				}
				if (0==string.Compare(attr, "*SCENE_FIRSTFRAME", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeInt();
					continue;
				}
				if (0==string.Compare(attr, "*SCENE_LASTFRAME", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeInt();
					continue;
				}
				if (0==string.Compare(attr, "*SCENE_FRAMESPEED", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeFloat();
					continue;
				}
				if (0==string.Compare(attr, "*SCENE_TICKSPERFRAME", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeFloat();
					continue;
				}
				if (0==string.Compare(attr, "*SCENE_BACKGROUND_STATIC", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeFloat();
					parser.ConsumeFloat();
					parser.ConsumeFloat();
					continue;
				}
				if (0==string.Compare(attr, "*SCENE_AMBIENT_STATIC", StringComparison.InvariantCultureIgnoreCase))
				{
					parser.ConsumeFloat();
					parser.ConsumeFloat();
					parser.ConsumeFloat();
					continue;
				}
				parser.UnknownLexemError();
			}
		}

		private void ParseComment(AseParser parser, Scene scene)
		{
			parser.Consume();
		}

		private void Parse3DsMaxSciiExport(AseParser parser, Scene scene)
		{
			parser.ConsumeInt();
		}

		#endregion
	}
}