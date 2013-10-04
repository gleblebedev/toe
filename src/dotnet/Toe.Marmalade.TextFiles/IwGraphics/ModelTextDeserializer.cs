using System.Collections.Generic;

using Autofac;

using OpenTK;

using Toe.Marmalade.IwGraphics;
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
	public class ModelTextDeserializer : ITextDeserializer
	{
		#region Constants and Fields

		private readonly IComponentContext context;

		#endregion

		#region Constructors and Destructors

		public ModelTextDeserializer(IComponentContext context)
		{
			this.context = context;
		}

		#endregion

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
				parser.UnknownLexemError();
			}

			return model;
		}

		public Mesh ParseMesh(TextParser parser)
		{
			parser.Consume("CMesh");
			parser.Consume("{");
			var mesh = this.context.Resolve<Mesh>();
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
					mesh.UseGeo = parser.ConsumeString();
					continue;
				}
				if (attribute == "useGroup")
				{
					parser.Consume();
					mesh.UseGroup = parser.ConsumeString();
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
					mesh.Surfaces.Add(this.ParseSurface(parser, mesh));
					continue;
				}
				if (attribute == "CIwModelExtSelSetFace")
				{
					this.ParseModelExtSelSetFace(parser, mesh);
					continue;
				}

				parser.UnknownLexemError();
			}
			mesh.CalculateTangents();
			return mesh;
		}

		#endregion

		#region Methods

		private static void ParseQuad(TextParser parser, IList<ComplexIndex> indices)
		{
			indices[0] = ParseTrisIndexes(parser);
			indices[1] = ParseTrisIndexes(parser);
			indices[2] = ParseTrisIndexes(parser);
			indices[3] = ParseTrisIndexes(parser);
		}

		private static void ParseTriangle(TextParser parser, IList<ComplexIndex> indices)
		{
			indices[0] = ParseTrisIndexes(parser);
			indices[1] = ParseTrisIndexes(parser);
			indices[2] = ParseTrisIndexes(parser);
		}

		private static ComplexIndex ParseTrisIndexes(TextParser parser)
		{
			parser.Consume("{");
			var i = new ComplexIndex();
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

		private static void ParseUVs(TextParser parser, Mesh mesh)
		{
			ListMeshStream<Vector2> uvs = null;

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
					if (setId == 0)
					{
						uvs = mesh.UV0;
					}
					else if (setId == 1)
					{
						uvs = mesh.UV1;
					}
					else
					{
						parser.Error("Only 2 UV streams supported");
					}
					continue;
				}
				if (attribute == "numUVs")
				{
					parser.Consume();
					if (uvs == null)
					{
						uvs = mesh.UV0;
					}
					uvs.Capacity = parser.ConsumeInt();
					continue;
				}
				if (attribute == "uv")
				{
					parser.Consume();
					if (uvs == null)
					{
						uvs = mesh.UV0;
					}
					uvs.Add(parser.ConsumeVector2());
					continue;
				}
				parser.UnknownLexemError();
			}
		}

		private static void ParseVertCols(TextParser parser, Mesh mesh)
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
				parser.UnknownLexemError();
			}
		}

		private static void ParseVertNorms(TextParser parser, ListMeshStream<Vector3> vertices)
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
				parser.UnknownLexemError();
			}
		}

		private static void ParseVerts(TextParser parser, ListMeshStream<Vector3> vertices)
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
				parser.UnknownLexemError();
			}
		}

		private ModelExtSelSetFace ParseModelExtSelSetFace(TextParser parser, Mesh mesh)
		{
			var sel = new ModelExtSelSetFace();
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
				parser.UnknownLexemError();
			}
			return sel;
		}

		private void ParseQuads(TextParser parser, ModelBlockPrimBase submesh)
		{
			var tmp = new ComplexIndex[4];
			int startIndex = submesh.Indices.Count;
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
					submesh.Indices.Capacity = startIndex + parser.ConsumeInt() * 2;
					continue;
				}
				if (attribute == "q")
				{
					parser.Consume();
					ParseQuad(parser, tmp);
					submesh.Indices.Add(tmp[0]);
					submesh.Indices.Add(tmp[2]);
					submesh.Indices.Add(tmp[1]);
					submesh.Indices.Add(tmp[0]);
					submesh.Indices.Add(tmp[3]);
					submesh.Indices.Add(tmp[2]);
					continue;
				}
				parser.UnknownLexemError();
			}
		}

		private Surface ParseSurface(TextParser parser, Mesh mesh)
		{
			parser.Consume("CSurface");
			parser.Consume("{");
			var surface = this.context.Resolve<ModelBlockPrimBase>();
			surface.Mesh = mesh;
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
					parser.ConsumeResourceReference(surface.Material);
					continue;
				}
				if (attribute == "CTris")
				{
					parser.Consume();
					parser.Consume("{");
					this.ParseTris(parser, surface);
					continue;
				}
				if (attribute == "CQuads")
				{
					parser.Consume();
					parser.Consume("{");
					this.ParseQuads(parser, surface);
					continue;
				}
				parser.UnknownLexemError();
			}
			return surface;
		}

		private void ParseTris(TextParser parser, ModelBlockPrimBase submesh)
		{
			var tmp = new ComplexIndex[3];
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
					submesh.Indices.Capacity = submesh.Indices.Count + parser.ConsumeInt();
					continue;
				}
				if (attribute == "t")
				{
					parser.Consume();
					ParseTriangle(parser, tmp);
					submesh.Indices.Add(tmp[0]);
					submesh.Indices.Add(tmp[2]);
					submesh.Indices.Add(tmp[1]);
					continue;
				}
				parser.UnknownLexemError();
			}
		}

		#endregion
	}
}