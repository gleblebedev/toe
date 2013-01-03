using Autofac;

using OpenTK;

using Toe.Utils.Marmalade;
using Toe.Utils.Marmalade.IwGraphics;
using Toe.Utils.Mesh;

#if WINDOWS_PHONE
using Microsoft.Xna.Framework;
#else

#endif

namespace Toe.Marmalade.TextFiles.IwGraphics
{
	/// <summary>
	/// Marmalade SDK .geo file parser.
	/// </summary>
	public class ModelTextSerializer : ITextSerializer
	{
		private readonly IComponentContext context;

		public ModelTextSerializer(IComponentContext context)
		{
			this.context = context;
		}

		#region Public Properties

		public string DefaultFileExtension
		{
			get
			{
				return ".geo";
			}
		}

		#endregion

		#region Public Methods and Operators

		public Managed Parse(TextParser parser, string defaultName)
		{
			var model = this.context.Resolve<Model>();
			model.Name = defaultName;

			if (parser.Lexem == "CMesh")
			{
				model.Meshes.Add(this.ParseMesh(parser));
				return model;
			}

			parser.Consume("CIwModel");
			parser.Consume("{");

			for (;;)
			{
				var attribute = parser.Lexem;
				if (attribute == "}")
				{
					parser.Consume();
					break;
				}
				if (attribute == "name")
				{
					parser.Consume();
					model.Name = parser.ConsumeString();
					continue;
				}
				if (attribute == "CMesh")
				{
					model.Meshes.Add(this.ParseMesh(parser));
					continue;
				}
				parser.UnknownLexem();
			}

			return model;
		}

		public StreamMesh ParseMesh(TextParser parser)
		{
			parser.Consume("CMesh");
			parser.Consume("{");
			var mesh = new StreamMesh();
			for (;;)
			{
				var attribute = parser.Lexem;
				if (attribute == "}")
				{
					parser.Consume();
					break;
				}
				if (attribute == "name")
				{
					parser.Consume();
					mesh.Name = parser.ConsumeString();
					continue;
				}
				if (attribute == "baseName")
				{
					parser.Consume();
					mesh.BaseName = parser.ConsumeString();
					continue;
				}
				if (attribute == "useGeo")
				{
					parser.Consume();
					mesh.useGeo = parser.ConsumeString();
					continue;
				}
				if (attribute == "useGroup")
				{
					parser.Consume();
					mesh.useGroup = parser.ConsumeString();
					continue;
				}
				if (attribute == "scale")
				{
					parser.Consume();
					mesh.Scale = parser.ConsumeFloat();
					continue;
				}
				if (attribute == "CVerts")
				{
					parser.Consume();
					parser.Consume("{");
					ParseVerts(parser, mesh.Vertices);
					continue;
				}
				if (attribute == "CVertNorms")
				{
					parser.Consume();
					parser.Consume("{");
					ParseVertNorms(parser, mesh.Normals);
					continue;
				}
				if (attribute == "CVertCols")
				{
					parser.Consume();
					parser.Consume("{");
					ParseVertCols(parser, mesh);
					continue;
				}
				if (attribute == "CUVs")
				{
					parser.Consume();
					parser.Consume("{");
					ParseUVs(parser, mesh);
					continue;
				}
				if (attribute == "CSurface")
				{
					parser.Consume();
					parser.Consume("{");
					this.ParseSurface(parser, mesh.CreateSubmesh());
					continue;
				}
				if (attribute == "CIwModelExtSelSetFace")
				{
					this.ParseModelExtSelSetFace(parser, mesh);
					continue;
				}

				parser.UnknownLexem();
			}
			return mesh;
		}

		#endregion

		#region Methods

		private static StreamSubmeshQuad ParseQuad(TextParser parser)
		{
			StreamSubmeshQuad t;
			t.A = ParseTrisIndexes(parser);
			t.B = ParseTrisIndexes(parser);
			t.C = ParseTrisIndexes(parser);
			t.D = ParseTrisIndexes(parser);
			return t;
		}

		private static StreamSubmeshTriangle ParseTriangle(TextParser parser)
		{
			StreamSubmeshTriangle t;
			t.A = ParseTrisIndexes(parser);
			t.B = ParseTrisIndexes(parser);
			t.C = ParseTrisIndexes(parser);
			return t;
		}

		private static StreamSubmeshTriangleIndexes ParseTrisIndexes(TextParser parser)
		{
			parser.Consume("{");
			StreamSubmeshTriangleIndexes i;
			i.Vertex = parser.ConsumeInt();
			parser.Skip(",");
			if (parser.Lexem == "}")
			{
				i.Normal = -1;
			}
			else
			{
				i.Normal = parser.ConsumeInt();
				parser.Skip(",");
			}
			if (parser.Lexem == "}")
			{
				i.UV0 = -1;
			}
			else
			{
				i.UV0 = parser.ConsumeInt();
				parser.Skip(",");
			}
			if (parser.Lexem == "}")
			{
				i.UV1 = -1;
			}
			else
			{
				i.UV1 = parser.ConsumeInt();
				parser.Skip(",");
			}
			if (parser.Lexem == "}")
			{
				i.Color = -1;
			}
			else
			{
				i.Color = parser.ConsumeInt();
			}
			parser.Consume("}");
			return i;
		}

		private static void ParseUVs(TextParser parser, StreamMesh mesh)
		{
			MeshStream<Vector2> uvs = null;

			for (;;)
			{
				var attribute = parser.Lexem;
				if (attribute == "}")
				{
					parser.Consume();
					break;
				}
				if (attribute == "setID")
				{
					parser.Consume();
					var setId = parser.ConsumeInt();
					uvs = mesh.EnsureUVStream(setId);
					continue;
				}
				if (attribute == "numUVs")
				{
					parser.Consume();
					if (uvs == null)
					{
						uvs = mesh.EnsureUVStream(0);
					}
					uvs.Capacity = parser.ConsumeInt();
					continue;
				}
				if (attribute == "uv")
				{
					parser.Consume();
					if (uvs == null)
					{
						uvs = mesh.EnsureUVStream(0);
					}
					uvs.Add(parser.ConsumeVector2());
					continue;
				}
				parser.UnknownLexem();
			}
		}

		private static void ParseVertCols(TextParser parser, StreamMesh mesh)
		{
			for (;;)
			{
				var attribute = parser.Lexem;
				if (attribute == "}")
				{
					parser.Consume();
					break;
				}
				if (attribute == "numVertCols")
				{
					parser.Consume();
					mesh.Colors.Capacity = parser.ConsumeInt();
					continue;
				}
				if (attribute == "col")
				{
					parser.Consume();
					mesh.Colors.Add(parser.ConsumeColor());
					continue;
				}
				parser.UnknownLexem();
			}
		}

		private static void ParseVertNorms(TextParser parser, MeshStream<Vector3> vertices)
		{
			for (;;)
			{
				var attribute = parser.Lexem;
				if (attribute == "}")
				{
					parser.Consume();
					break;
				}
				if (attribute == "numVertNorms")
				{
					parser.Consume();
					vertices.Capacity = parser.ConsumeInt();
					continue;
				}
				if (attribute == "vn")
				{
					parser.Consume();
					vertices.Add(parser.ConsumeVector3());
					continue;
				}
				parser.UnknownLexem();
			}
		}

		private static void ParseVerts(TextParser parser, MeshStream<Vector3> vertices)
		{
			for (;;)
			{
				var attribute = parser.Lexem;
				if (attribute == "}")
				{
					parser.Consume();
					break;
				}
				if (attribute == "numVerts")
				{
					parser.Consume();
					vertices.Capacity = parser.ConsumeInt();
					continue;
				}
				if (attribute == "v")
				{
					parser.Consume();
					vertices.Add(parser.ConsumeVector3());
					continue;
				}
				parser.UnknownLexem();
			}
		}

		private ModelExtSelSetFace ParseModelExtSelSetFace(TextParser parser, IMesh mesh)
		{
			ModelExtSelSetFace sel = new ModelExtSelSetFace();
			parser.Consume("CIwModelExtSelSetFace");
			parser.Consume("{");
			for (;;)
			{
				var attribute = parser.Lexem;
				if (attribute == "}")
				{
					parser.Consume();
					break;
				}
				if (attribute == "name")
				{
					parser.Consume();
					sel.Name = parser.ConsumeString();
					continue;
				}
				if (attribute == "otzOfs")
				{
					parser.Consume();
					sel.otzOfs = parser.ConsumeFloat();
					continue;
				}
				if (attribute == "f")
				{
					parser.Consume();
					sel.F.Add(parser.ConsumeInt());
					continue;
				}
				parser.UnknownLexem();
			}
			return sel;
		}

		private void ParseQuads(TextParser parser, StreamSubmesh submesh)
		{
			for (;;)
			{
				var attribute = parser.Lexem;
				if (attribute == "}")
				{
					parser.Consume();
					break;
				}
				if (attribute == "numQuads")
				{
					parser.Consume();
					submesh.Tris.Capacity = submesh.Tris.Count + parser.ConsumeInt() * 2;
					continue;
				}
				if (attribute == "q")
				{
					parser.Consume();
					var streamSubmeshQuad = ParseQuad(parser);
					submesh.Tris.Add(
						new StreamSubmeshTriangle { A = streamSubmeshQuad.A, B = streamSubmeshQuad.B, C = streamSubmeshQuad.C });
					submesh.Tris.Add(
						new StreamSubmeshTriangle { A = streamSubmeshQuad.A, B = streamSubmeshQuad.C, C = streamSubmeshQuad.D });
					continue;
				}
				parser.UnknownLexem();
			}
		}

		private void ParseSurface(TextParser parser, ISubMesh submesh)
		{
			for (;;)
			{
				var attribute = parser.Lexem;
				if (attribute == "}")
				{
					parser.Consume();
					break;
				}
				if (attribute == "material")
				{
					parser.Consume();
					submesh.Material = parser.ConsumeString();
					continue;
				}
				if (attribute == "CTris")
				{
					parser.Consume();
					parser.Consume("{");
					this.ParseTris(parser, (StreamSubmesh)submesh);
					continue;
				}
				if (attribute == "CQuads")
				{
					parser.Consume();
					parser.Consume("{");
					this.ParseQuads(parser, (StreamSubmesh)submesh);
					continue;
				}
				parser.UnknownLexem();
			}
		}

		private void ParseTris(TextParser parser, StreamSubmesh submesh)
		{
			for (;;)
			{
				var attribute = parser.Lexem;
				if (attribute == "}")
				{
					parser.Consume();
					break;
				}
				if (attribute == "numTris")
				{
					parser.Consume();
					submesh.Tris.Capacity = submesh.Tris.Count + parser.ConsumeInt();
					continue;
				}
				if (attribute == "t")
				{
					parser.Consume();
					submesh.Tris.Add(ParseTriangle(parser));
					continue;
				}
				parser.UnknownLexem();
			}
		}

		#endregion
	}
}