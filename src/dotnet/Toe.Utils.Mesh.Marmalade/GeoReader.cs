using System.Globalization;
using System.IO;

#if WINDOWS_PHONE
using Microsoft.Xna.Framework;
#else
using OpenTK;
#endif

namespace Toe.Utils.Mesh.Marmalade
{
	/// <summary>
	/// Marmalade SDK .geo file parser.
	/// </summary>
	public class GeoReader : IMeshReader
	{
		#region Public Methods and Operators

		/// <summary>
		/// Load mesh from stream.
		/// </summary>
		/// <param name="stream">Stream to read from.</param>
		/// <returns>Complete parsed mesh.</returns>
		public IMesh Load(Stream stream)
		{
			var mesh = new StreamMesh();

			using (var source = new StreamReader(stream))
			{
				var parser = new TextParser(source);

				parser.Consume("CIwModel");
				parser.Consume("{");

				for (;;)
				{
					var attribute = parser.GetLexem();
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
					if (attribute == "CMesh")
					{
						parser.Consume();
						parser.Consume("{");
						this.ParseMesh(parser, mesh);
						continue;
					}
					throw new TextParserException(string.Format(CultureInfo.InvariantCulture, "Unknown attribute {0}", attribute));
				}
			}

			return mesh;
		}

		#endregion

		#region Methods

		private void ParseMesh(TextParser parser, StreamMesh mesh)
		{
			for (;;)
			{
				var attribute = parser.GetLexem();
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
				throw new TextParserException(string.Format(CultureInfo.InvariantCulture, "Unknown attribute {0}", attribute));
			}
		}

		private void ParseSurface(TextParser parser, ISubMesh submesh)
		{
			for (;;)
			{
				var attribute = parser.GetLexem();
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
					this.ParseTris(parser,  (StreamSubmesh)submesh);
					continue;
				}
				throw new TextParserException(string.Format(CultureInfo.InvariantCulture, "Unknown attribute {0}", attribute));
			}
		}

		private static StreamSubmeshTiangle ParseTriangle(TextParser parser)
		{
			StreamSubmeshTiangle t;
			t.A = ParseTrisIndexes(parser);
			t.B = ParseTrisIndexes(parser);
			t.C = ParseTrisIndexes(parser);
			return t;
		}

		private void ParseTris(TextParser parser, StreamSubmesh submesh)
		{
			for (;;)
			{
				var attribute = parser.GetLexem();
				if (attribute == "}")
				{
					parser.Consume();
					break;
				}
				if (attribute == "numTris")
				{
					parser.Consume();
					submesh.Tris.Capacity = parser.ConsumeInt();
					continue;
				}
				if (attribute == "t")
				{
					parser.Consume();
					submesh.Tris.Add(ParseTriangle(parser));
					continue;
				}
				throw new TextParserException(string.Format(CultureInfo.InvariantCulture, "Unknown attribute {0}", attribute));
			}
		}

		private static StreamSubmeshTiangleIndexes ParseTrisIndexes(TextParser parser)
		{
			parser.Consume("{");
			StreamSubmeshTiangleIndexes i;
			i.Vertex = parser.ConsumeInt();
			parser.Skip(",");
			i.Normal = parser.ConsumeInt();
			parser.Skip(",");
			i.UV0 = parser.ConsumeInt();
			parser.Skip(",");
			i.UV1 = parser.ConsumeInt();
			parser.Skip(",");
			i.Color = parser.ConsumeInt();
			parser.Consume("}");
			return i;
		}

		private static void ParseUVs(TextParser parser, StreamMesh mesh)
		{
			MeshStream<Vector2> uvs = null;

			for (;;)
			{
				var attribute = parser.GetLexem();
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
				throw new TextParserException(string.Format(CultureInfo.InvariantCulture, "Unknown attribute {0}", attribute));
			}
		}

		private static void ParseVertCols(TextParser parser, StreamMesh mesh)
		{
			for (;;)
			{
				var attribute = parser.GetLexem();
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
				throw new TextParserException(string.Format(CultureInfo.InvariantCulture, "Unknown attribute {0}", attribute));
			}
		}

		private static void ParseVertNorms(TextParser parser, MeshStream<Vector3> vertices)
		{
			for (;;)
			{
				var attribute = parser.GetLexem();
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
				throw new TextParserException(string.Format(CultureInfo.InvariantCulture, "Unknown attribute {0}", attribute));
			}
		}

		private static void ParseVerts(TextParser parser, MeshStream<Vector3> vertices)
		{
			for (;;)
			{
				var attribute = parser.GetLexem();
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
				throw new TextParserException(string.Format(CultureInfo.InvariantCulture, "Unknown attribute {0}", attribute));
			}
		}

		#endregion
	}
}